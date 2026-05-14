# Contributing to WinMCP-Modules

Thanks for contributing. This repository holds the official modules for the [WinMCP](https://github.com/ryanhebert/WinMCP) platform.

## No CLA here

Unlike the `WinMCP` platform repo, this modules repo does **not** require a CLA. All contributions are accepted under the standard Apache 2.0 license — your copyright stays yours, the project gets a perpetual permissive license, no relicensing reservation.

## Code of Conduct

[Contributor Covenant 2.1](CODE_OF_CONDUCT.md).

## Development setup

Requires .NET 8 SDK and a recent published `WinMcp.ModuleSdk` NuGet package.

```bash
git clone https://github.com/ryanhebert/WinMCP-Modules.git
cd WinMCP-Modules
dotnet build
```

To test a module locally against a running WinMCP instance, build the module's `.dll`, drop it into `C:\Program Files\WinMCP\modules\<name>\` along with the `module.json`, and restart the `WinMcp` Windows Service.

## Submitting a new module

1. Open an issue first to discuss whether your module belongs in the *official* set or should live in your own repository. Most third-party modules belong in their own repo, with a registry pointer here.
2. If an official module is appropriate, create `src/<YourModule>/` with:
   - `<YourModule>.csproj` referencing `WinMcp.ModuleSdk` from NuGet
   - `module.json` per [docs/MANIFEST.md](docs/MANIFEST.md)
   - `<YourModule>Module.cs` implementing `IMcpModule`
   - Your tool / prompt / resource source files
3. Add a CHANGELOG entry under `src/<YourModule>/CHANGELOG.md`
4. Open a pull request

## Modifying an existing module

1. Branch from `main`
2. Update the module's source under `src/<ModuleName>/`
3. Bump the version in the module's `<Module>.csproj` and `module.json` if shipping a release
4. Update `src/<ModuleName>/CHANGELOG.md`
5. Open a pull request

## Branching and PRs

- `main` is the integration branch
- One module change per PR; don't bundle changes across modules
- Reference the issue number if applicable

## Reporting bugs

Open an issue and tag it with the affected module's label (e.g., `module:math`). Include:
- Module version (from the dashboard or `<InstallDir>\modules\<name>\module.json`)
- WinMCP platform version
- Reproduction steps

Security issues — see [SECURITY.md](SECURITY.md).
