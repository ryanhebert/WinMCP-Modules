# Building a WinMCP module

A module is a .NET class library that:
1. References `WinMcp.ModuleSdk` (NuGet)
2. Implements `IMcpModule` to register MCP tools/prompts/resources
3. Ships a `module.json` manifest alongside its DLL

This guide walks through the minimum needed to ship a working module.

## Project setup

```bash
dotnet new classlib -n WinMcpModule.Hello -f net8.0
cd WinMcpModule.Hello
dotnet add package WinMcp.ModuleSdk
dotnet add package ModelContextProtocol --version 1.2.0
```

Edit `WinMcpModule.Hello.csproj` to set the target framework and assembly name:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <AssemblyName>HelloModule</AssemblyName>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <Version>1.0.0</Version>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="WinMcp.ModuleSdk" Version="1.0.0" />
    <PackageReference Include="ModelContextProtocol" Version="1.2.0" />
  </ItemGroup>
</Project>
```

## Implement `IMcpModule`

`HelloModule.cs`:

```csharp
using Microsoft.Extensions.Logging;
using WinMcp.ModuleSdk;

namespace WinMcpModule.Hello;

public sealed class HelloModule : IMcpModule
{
    public ModuleMetadata Metadata { get; } = new()
    {
        Name = "hello",
        Version = "1.0.0",
        DisplayName = "Hello",
        Description = "A minimal example module"
    };

    public void Configure(ModuleConfigurationContext ctx)
    {
        ctx.Logger.LogInformation("Hello module configuring");
        ctx.Mcp.WithToolsFromAssembly(typeof(HelloTools).Assembly);
    }
}
```

## Add MCP tools

`HelloTools.cs`:

```csharp
using System.ComponentModel;
using ModelContextProtocol.Server;

namespace WinMcpModule.Hello;

[McpServerToolType]
public static class HelloTools
{
    [McpServerTool, Description("Says hello to the named person.")]
    public static string Greet(string name) => $"Hello, {name}!";
}
```

## Write the manifest

`module.json` (alongside the DLL in the release artifact):

```json
{
  "name": "hello",
  "version": "1.0.0",
  "displayName": "Hello",
  "description": "A minimal example module",
  "assembly": "HelloModule.dll",
  "entryType": "WinMcpModule.Hello.HelloModule",
  "mountPath": "/hello",
  "minPlatformVersion": "1.0.0",
  "maturity": "experimental",
  "publisher": {
    "name": "Your name",
    "url": "https://github.com/your-username"
  },
  "homepage": "https://github.com/your-username/winmcp-module-hello",
  "license": "Apache-2.0",
  "tags": ["example"]
}
```

`name` in the manifest, the `Metadata.Name` in code, and the directory name on disk must all match.

## Build + package

```bash
dotnet build -c Release
mkdir -p artifact
cp bin/Release/net8.0/HelloModule.dll module.json artifact/
cd artifact && zip ../HelloModule-v1.0.0.zip *
```

## Test against a local WinMCP instance

```powershell
# On the Windows machine running WinMCP:
Expand-Archive HelloModule-v1.0.0.zip -DestinationPath "C:\Program Files\WinMCP\modules\hello\"
sc.exe stop WinMcp
sc.exe start WinMcp
```

After restart, the module is loaded. Verify:
- `http://localhost:52080/info` lists `hello` under `modules`
- `http://localhost:52080/hello/mcp` accepts MCP requests
- The platform's dashboard shows the Hello module card

## Publishing

You have two options:

**Option A — Host in your own repo (recommended for third-party modules).** Cut a GitHub release with `HelloModule-v1.0.0.zip` attached. Share the release URL; users install via the platform's "Install module" UI by pasting the URL.

**Option B — Submit to the official `WinMCP-Modules` repo.** Open an issue here describing the module; if accepted, contribute it under `src/Hello/`. Releases go through the repo's CI release-module workflow. See [PUBLISHING.md](PUBLISHING.md).

## Best practices

- **Stateless tools** are preferable. If your module needs state, store it via a service registered in `ctx.Services` rather than static fields, so multiple instances of the module (or future hot-reload) work correctly.
- **Logging** uses `ctx.Logger`. Don't `Console.WriteLine` — operators read the platform's log, not stdout.
- **Config** for module-specific values (API keys, base URLs) goes in `config.json`'s `modules.<name>.moduleConfig` section, available to you as `ctx.ModuleConfig`.
- **Auth** for your module's `/mcp` endpoint is platform-managed. Don't write your own auth middleware; declare requirements via the manifest's `auth` field.
- **Don't reach into platform internals.** The only types you should depend on from the platform are those exposed by `WinMcp.ModuleSdk`. The platform's own assemblies aren't accessible from your `AssemblyLoadContext`.
