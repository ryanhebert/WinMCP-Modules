# Module manifest schema (`module.json`)

Every WinMCP module ships a `module.json` alongside its DLL. The platform parses this at load time and validates required fields strictly; unknown top-level keys produce a warning but do not fail the load (forward compatibility).

## Example

```json
{
  "name": "math",
  "version": "1.0.0",
  "displayName": "Math",
  "description": "Arithmetic tools, constants, identities",
  "assembly": "MathModule.dll",
  "entryType": "WinMcpModule.Math.MathModule",
  "mountPath": "/math",
  "minPlatformVersion": "1.0.0",
  "maturity": "demo",
  "publisher": {
    "name": "WinMCP Project",
    "url": "https://github.com/ryanhebert/WinMCP",
    "verified": true
  },
  "homepage": "https://github.com/ryanhebert/WinMCP-Modules/tree/main/src/Math",
  "license": "Apache-2.0",
  "tags": ["math", "demo", "tools"],
  "auth": null
}
```

## Required fields

| Field | Type | Description |
|---|---|---|
| `name` | string | Module identifier. Lowercase, alphanumeric + hyphens. Must match the directory name on disk. Used as the URL prefix. |
| `version` | string | SemVer (e.g. `1.0.0`, `2.3.1-rc1`). |
| `displayName` | string | Human-friendly name shown on the dashboard. |
| `assembly` | string | DLL filename relative to the module directory (e.g. `MathModule.dll`). |
| `entryType` | string | Fully-qualified type name of the `IMcpModule` implementation (e.g. `WinMcpModule.Math.MathModule`). |
| `mountPath` | string | URL prefix where the module's MCP transport is mounted. Must start with `/`. Conventionally `/<name>`. |
| `minPlatformVersion` | string | Minimum WinMCP platform version required (SemVer). Module refuses to load on older platforms. |

## Recommended fields

| Field | Type | Description |
|---|---|---|
| `description` | string | One-paragraph summary shown on the dashboard. |
| `maturity` | enum | `"demo"` (testing only), `"experimental"` (real functionality, unstable), `"production"` (hardened). v1.0 displays this; v1.1 enforces safety rails. |
| `publisher` | object | `{ "name", "url", "verified" }` — shown on the dashboard. `verified: true` requires a future trusted-publisher signature; in v1.0 it's purely informational. |
| `homepage` | string | URL to the module's source or documentation. |
| `license` | string | SPDX license identifier of the module's source code. |
| `tags` | string[] | Searchable tags for the dashboard's module-browse UI (v1.1). |

## Reserved fields (v1.0 accepts, v1.1+ enforces)

| Field | Type | Description |
|---|---|---|
| `auth` | object or null | Per-module auth override. `null` (or omitted) means inherit `mcp.defaultAuth` from platform config. See [auth section](#auth-override) below. |
| `requiredEntitlements` | string[] | License-based entitlements required to load this module. Empty in v1.0 (no entitlement system yet). |
| `minMcpProtocolVersion` | string | Minimum MCP protocol version (e.g. `"2025-06-18"`). v1.1 refuses to load modules incompatible with the platform's supported MCP versions. |
| `maxMcpProtocolVersion` | string \| null | Upper bound on MCP protocol version. |
| `signature` | object \| null | Authenticode + manifest signature (v1.2+). |

## Auth override

The `auth` field, when present, overrides the platform-level MCP default for this module specifically.

| Value | Meaning |
|---|---|
| `null` or omitted | Inherit `mcp.defaultAuth` from `config.json`. Most modules. |
| `{ "mode": "none" }` | No auth for this module's `/mcp` endpoint. Operator must opt in via `allowAuthDowngrade: true` if the platform default is stricter. |
| `{ "mode": "demo" }` | Use platform-issued tokens. Only valid if the platform's identity providers are configured to support demo mode. |
| `{ "mode": "oidc", "providerRef": "name", "requiredScopes": [...], "requiredClaims": {...} }` | Use a specific OIDC provider configured in the platform. May add scope/claim requirements beyond the platform default. |

The platform resolves the *effective* auth config at module load time and stores it; the resolution is shown on each module's dashboard page.

## Validation rules

- All required fields must be present and of the correct type.
- `name` must match the regex `^[a-z][a-z0-9-]{0,62}$`.
- `version` must be valid SemVer.
- `mountPath` must start with `/`, not contain `..`, not be `/`, and not collide with platform-reserved paths (`/info`, `/health`, `/logs`, etc.).
- `auth.mode` (if present) must be one of `"none"`, `"demo"`, `"oidc"`.
- `maturity` (if present) must be one of `"demo"`, `"experimental"`, `"production"`.
- `entryType` must resolve to a class implementing `WinMcp.ModuleSdk.IMcpModule` in the named assembly. Mismatch fails the load.

Failed validation produces a clear error in the platform log; the module is skipped, and other modules continue to load.

## Versioning the schema

Manifest schema is versioned implicitly via the WinMCP platform version. Adding new optional fields is a non-breaking change. Renaming or removing fields is a major-version change to the platform.
