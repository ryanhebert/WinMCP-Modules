# WinMCP-Modules

Official modules for the [WinMCP](https://github.com/ryanhebert/WinMCP) platform.

Each module is a self-contained MCP server that runs inside the WinMCP host process. Modules expose their tools, prompts, and resources at `/<module>/mcp` on the platform's HTTP surface.

## Available modules

| Module | Latest | Description |
|---|---|---|
| [math](src/Math) | [`math-v1.0.0`](https://github.com/ryanhebert/WinMCP-Modules/releases/tag/math-v1.0.0) | Arithmetic tools, constants, identities |

(More to come.)

## Installing a module

Download the module's release zip from its GitHub release, extract it into `C:\Program Files\WinMCP\modules\<name>\` (directory name must match the `name` field in `module.json`), and restart the `WinMcp` service. The dashboard will then show the module with its mount path, maturity, and tool/prompt/resource counts.

Once a module is installed and declares an `updateSource` block in its manifest, the dashboard's per-module **Upgrade ↑** button handles subsequent version bumps end-to-end — no manual download needed.

## Building your own module

See [docs/BUILDING-A-MODULE.md](docs/BUILDING-A-MODULE.md) for the developer guide. Short version:

```bash
dotnet new classlib -n MyModule
cd MyModule
dotnet add package WinMcp.ModuleSdk
# implement IMcpModule; add MCP tools/prompts/resources
dotnet pack
```

Then ship a GitHub release containing the DLL + `module.json` zipped together. Users install via paste-a-URL in the WinMCP dashboard, or by manual drop-in.

You can host modules in your own repository; you don't need to contribute them here. This repo holds the curated set of *official* modules.

## Module manifest schema

See [docs/MANIFEST.md](docs/MANIFEST.md). Every module ships a `module.json` alongside its DLL.

## Releasing a module from this repo

See [docs/PUBLISHING.md](docs/PUBLISHING.md). Per-module tagged releases — `math-v1.0.0`, `weather-v0.2.1`, etc. The `modules.json` registry index auto-updates on each release.

## Contributing

We welcome new official modules and improvements to existing ones. See [CONTRIBUTING.md](CONTRIBUTING.md). Unlike the platform repository, this repo does **not** require a CLA — all contributions are accepted under the standard Apache 2.0 license.

## License

Apache 2.0 — see [LICENSE](LICENSE). Each module's own dependencies may have additional license obligations; see the module's individual `LICENSE` or `NOTICE` if present.

## Security

Vulnerabilities in any module here should be reported per [SECURITY.md](SECURITY.md), not via public issues.
