using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MobSwitcher.Cli.Tests
{
  public class DoneCmdTests
  {
    protected readonly StartupFixture fixture;

    public DoneCmdTests()
    {
      fixture = new StartupFixture();
    }

    [Theory]
    [InlineData("done")]
    [InlineData("DONE")]
    [InlineData("d")]
    [InlineData("D")]
    public async Task ShoulDoneOnArgument(string cmd)
    {
      await fixture.Run(cmd);

      fixture.FakeShellCmdService.Called.Should().BeGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task ShouldNotDoneWhenNotMobbing()
    {
      fixture.FakeShellCmdService.Add("git branch", string.Empty);

      await fixture.Run("done");

      fixture.FakeShellCmdService.Commands.Should().NotContain("git fetch --prune");
    }

    [Fact]
    public async Task ShouldDeleteSessionBranchWhenAlreadyDone()
    {
      fixture.FakeShellCmdService.Add("git branch", "* mob-session");
      fixture.FakeShellCmdService.Add("git branch --remotes", string.Empty);

      await fixture.Run("done");

      fixture.FakeSayService.Says.Last().Should().Contain("someone else already ended your mob session");
    }

    [Fact]
    public async Task ShouldAddAll()
    {
      fixture.FakeShellCmdService.Add("git branch", "* mob-session");
      fixture.FakeShellCmdService.Add("git branch --remotes", "  origin/mob-session");

      await fixture.Run("done");


    }
  }
}