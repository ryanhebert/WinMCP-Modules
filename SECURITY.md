# Security Policy

## Reporting a vulnerability

Please report vulnerabilities privately, **not via public GitHub issues**.

Contact: open a [private security advisory on GitHub](https://github.com/ryanhebert/WinMCP-Modules/security/advisories/new), or email the project maintainer (see the GitHub profile at <https://github.com/ryanhebert>).

Include:
- The affected module name and version
- WinMCP platform version where the issue reproduces
- Reproduction steps or proof-of-concept
- Impact assessment

## Scope

In scope:
- Modules in `src/` of this repository (currently: `math`)

Out of scope:
- The WinMCP platform itself — report to <https://github.com/ryanhebert/WinMCP/security/advisories>
- Third-party modules hosted in repositories other than this one — contact those projects directly
- Operational misconfiguration of the deploying platform

## Response process

Same as the WinMCP platform: ack within 5 business days, triage within 10, coordinated disclosure timeline agreed with the reporter (default 90 days from confirmation), CVE assignment for confirmed issues warranting it.
