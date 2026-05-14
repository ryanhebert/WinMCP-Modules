# Publishing a module from this repo

Modules in this repository are released independently. Each module gets its own tagged releases with the naming convention `<module>-v<MAJOR>.<MINOR>.<PATCH>` (e.g., `math-v1.0.0`, `weather-v0.2.1`).

## Release flow

1. Make sure the module's `Version` in `<Module>.csproj`, the `version` field in `module.json`, and the entry in `src/<Module>/CHANGELOG.md` all match the version you intend to ship.
2. Open a PR that bumps these values. Merge to `main` after review.
3. Tag the merge commit on `main`:
   ```bash
   git tag math-v1.0.0
   git push origin math-v1.0.0
   ```
4. The `release-module.yml` workflow detects the tag prefix, builds only the matching module, packages it as `<Module>Module-v<version>.zip`, and creates a GitHub release with the zip attached.
5. The `update-index.yml` workflow regenerates `modules.json` at the repo root, adding or updating the entry for the released module. It commits the change directly to `main`.

## What goes in the release zip

```
<Module>Module-v<version>.zip
├─ <Module>Module.dll
├─ module.json
└─ (any module-specific assets — readme, optional dependencies, etc.)
```

The DLL and `module.json` are mandatory. The platform's installer extracts the zip into `<InstallDir>\modules\<name>\` verbatim, so the layout must match what the platform expects.

## Versioning rules

Follow SemVer:
- **Patch** (`1.0.0` → `1.0.1`): bug fixes, no API changes
- **Minor** (`1.0.0` → `1.1.0`): new tools/prompts/resources, no breaking changes
- **Major** (`1.0.0` → `2.0.0`): breaking changes — removed/renamed tools, changed argument shapes, changed minimum platform version

Bump the minimum platform version (`minPlatformVersion` in `module.json`) whenever the module starts to require a feature only available in a newer platform release.

## Pre-release tags

For RC builds, use the `-rcN` suffix: `math-v1.0.0-rc1`. The release workflow recognizes these and marks the GitHub release as a pre-release. Pre-releases are excluded from `modules.json`'s "latest version" tracking — only stable releases populate the registry.

## Changelog

Each module maintains its own `src/<Module>/CHANGELOG.md`. Format:

```markdown
# Changelog — <ModuleName>

## [v1.0.1] — 2026-MM-DD

### Fixed
- ...

## [v1.0.0] — 2026-MM-DD

Initial release.
```

The release notes published to GitHub are auto-generated from this changelog section by the release workflow.

## Adding a new module's release plumbing

When you add a new module to `src/<NewModule>/`, also:
1. Add the module's name to the tag-prefix matcher in `.github/workflows/release-module.yml`
2. Add a `src/<NewModule>/CHANGELOG.md` with a `## [Unreleased]` section
3. Run `dotnet build` to verify it builds against the current `WinMcp.ModuleSdk` version

The CI workflow at `.github/workflows/ci.yml` automatically picks up any new `src/*/*.csproj` and includes it in the build matrix; no additional CI config is needed for builds.

## Removing a module from the registry

To deprecate a module:
1. Cut a final release with `"deprecated": true` set in `module.json` (the platform's dashboard will surface a deprecation warning).
2. Move the module's source to `src/_deprecated/<OldName>/`.
3. The next `update-index.yml` run will remove it from `modules.json`'s `officialModules` array; the GitHub releases remain available for users who still depend on it.
