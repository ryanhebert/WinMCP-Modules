# Changelog (repo-level)

This top-level changelog tracks changes to the repository structure, CI, and the `modules.json` registry index. Each module has its own changelog at `src/<ModuleName>/CHANGELOG.md` tracking that module's tagged releases.

## [Unreleased]

_(no entries yet)_

## [2026-05-15] — math-v1.0.0

First module release. Pairs with WinMCP platform v1.0.0.

### Added
- `.github/workflows/release-module.yml` — tag-driven per-module release pipeline. Triggers on `<module>-v*` tags, builds the matching module, zips `<Module>Module.dll` + `module.json` as `<Module>Module-v<version>.zip`, and attaches to a GitHub Release. `-rc*` tags mark the release as a pre-release.
- `.github/workflows/update-index.yml` — regenerates `modules.json` after each non-prerelease publish, pairing each `src/*/module.json` with its latest stable `<name>-v*` tag.
- Initial repo scaffolding (LICENSE, README, CONTRIBUTING, CoC, SECURITY)
- `modules.json` registry index format
- Manifest schema documentation

### Released
- `math-v1.0.0` — see [src/Math/CHANGELOG.md](src/Math/CHANGELOG.md). The `modules.json` registry entry is populated automatically by `update-index.yml` once the release is published.
