namespace BalkanAir.SmokeTests;

/// <summary>
/// Foundation smoke test: confirms the .NET 8 build + test pipeline runs in CI.
/// Real test suites are added alongside each migrated layer.
/// </summary>
public class ToolchainSmokeTests
{
    [Fact]
    public void Toolchain_Builds_And_Runs_Tests()
    {
        Assert.Equal(8, Environment.Version.Major);
    }
}
