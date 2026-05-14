using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WinMcp.ModuleSdk;

namespace WinMcpModule.Math;

/// <summary>
/// WinMCP module providing arithmetic tools, prompts, and resources.
/// Registered MCP capabilities are surfaced at <c>/math/mcp</c>.
/// </summary>
[McpModule]
public sealed class MathModule : IMcpModule
{
    public ModuleMetadata Metadata { get; } = new()
    {
        Name = "math",
        Version = "1.0.0",
        DisplayName = "Math",
        Description = "Arithmetic tools, constants, identities, and prime-list resources."
    };

    public void Configure(ModuleConfigurationContext ctx)
    {
        ctx.Logger.LogInformation(
            "Math module configuring (platform={Platform}, mcpAuth={Auth})",
            ctx.Platform.PlatformVersion, ctx.Platform.McpDefaultAuthMode);

        var asm = typeof(MathTools).Assembly;
        ctx.Mcp
            .WithToolsFromAssembly(asm)
            .WithPromptsFromAssembly(asm)
            .WithResourcesFromAssembly(asm);
    }
}
