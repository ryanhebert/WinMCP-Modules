# Changelog — Math module

All notable changes to the Math WinMCP module.

## [Unreleased]

_(no entries yet)_

## [math-v1.0.0] — 2026-05-15

First tagged release. Pairs with WinMCP platform v1.0.0.

### Added
- Initial port from the standalone `math-mcp` server (v1.0.24)
- Implements `IMcpModule` against `WinMcp.ModuleSdk` 1.0.0-alpha.1
- Tools: `add`, `subtract`, `multiply`, `divide`
- Prompts: `solve-expression`, `compare-numbers`
- Resources: `math://constants`, `math://identities`, `math://primes`
- `updateSource` block — points at this repo's `math-v*` releases with
  the `MathModule-{version}.zip` asset, so the WinMCP dashboard's
  per-module Upgrade button picks up newer math releases automatically.

### Notes
- Mounts at `/math/mcp` on the WinMCP platform
- `maturity: demo` — for testing MCP flows, not production data
