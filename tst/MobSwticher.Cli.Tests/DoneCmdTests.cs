using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MobSwitcher.Cli.Tests.Fakes;
using Xunit;

namespace MobSwitcher.Cli.Tests
{
  public class DoneCmdTests
  {
    protected readonly StartupFixture fixture;
    private readonly FakeShellCmdService fakeShellCmd;
    private readonly FakeSayService fakeSay;

    public DoneCmdTests()
    {
      this.fixture = new StartupFixture();
      fakeShellCmd = this.fixture.FakeShellCmdService;
      fakeSay = this.fixture.FakeSayService;
    }

    [Theory]
    [InlineData("done")]
    [InlineData("DONE")]
    [InlineData("d")]
    [InlineData("D")]
    public async Task ShoulDoneOnArgument(string cmd)
    {
      var result = await fixture.Run(cmd);

      fakeShellCmd.Called.Should().BeGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task ShouldNotDoneWhenNotMobbing()
    {
      fakeShellCmd.ShellCmdResponses.Add("git branch", string.Empty);

      await fixture.Run("done");

      fakeShellCmd.Commands.Should().NotContain("git fetch --prune");
    }

    [Fact]
    public async Task ShouldDeleteSessionBranchWhenAlreadyDone()
    {
      fakeShellCmd.ShellCmdResponses.Add("git branch", "* mob-session");
      fakeShellCmd.ShellCmdResponses.Add("git branch --remotes", string.Empty);

      await fixture.Run("done");

      fakeSay.Says.Last().Should().Contain("someone else already ended your mob session");
    }

    [Fact]
    public async Task ShouldAddAll()
    {
      fakeShellCmd.ShellCmdResponses.Add("git branch", "* mob-session");
      fakeShellCmd.ShellCmdResponses.Add("git branch --remotes", "  origin/mob-session");

      await fixture.Run("done");

      
    }
  }
}