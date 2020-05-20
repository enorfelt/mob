using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MobSwitcher.Cli.Tests.Fakes;
using Xunit;

namespace MobSwitcher.Cli.Tests {
  public class StartCmdTests {
    private StartupFixture fixture;
    private FakeShellCmdService fakeCmdService;
    private FakeSayService fakeSayService;

    public StartCmdTests() {
      fixture = new StartupFixture();
      fakeCmdService = fixture.FakeShellCmdService;
      fakeSayService = fixture.FakeSayService;
    }

    [Theory]
    [InlineData("start")]
    [InlineData("START")]
    [InlineData("s")]
    [InlineData("S")]
    public async Task ShouldStartOnArgument(string cmd) {
      var result = await fixture.Run(cmd);

      fixture.FakeShellCmdService.Called.Should().BeGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task ShouldRejoinWhenHasSessionBranchLocalAndRemote() {
      fakeCmdService.Add("git status --short", string.Empty);
      fakeCmdService.Add("git branch", "  mob-session");
      fakeCmdService.Add("git branch --remotes", "  origin/mob-session");

      var result = await fixture.Run("start");

      fakeSayService.Says.Should().Contain("rejoining mob session");
    }

    [Fact]
    public async Task ShouldCreateWhenSessionBranchDontExists() {
      fakeCmdService.Add("git status --short", string.Empty);
      fakeCmdService.Add("git branch", string.Empty);
      fakeCmdService.Add("git branch --remotes", string.Empty);

      var result = await fixture.Run("start");

      fakeSayService.Says.Any(r => r.Contains("create mob-session")).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldJoinWhenLocalSessionBranchDontExists() {
      fakeCmdService.Add("git status --short", string.Empty);
      fakeCmdService.Add("git branch", string.Empty);
      fakeCmdService.Add("git branch --remotes", "  origin/mob-session");

      var result = await fixture.Run("start");

      fakeSayService.Says.Any(r => r.Contains("joining mob session")).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldStartNewWhenLocalSessionBranchExistsAndRemoteDont() {
      fakeCmdService.Add("git status --short", string.Empty);
      fakeCmdService.Add("git branch", "  mob-session");
      fakeCmdService.Add("git branch --remotes", string.Empty);

      var result = await fixture.Run("start");

      fakeSayService.Says.Any(r => r.Contains("purging local branch and start new")).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldNotDeleteMobBranchWhenOnMobBranchAndMobbing()
    {
      fakeCmdService.Add("git status --short", string.Empty);
      fakeCmdService.Add("git branch", "* mob-session");
      fakeCmdService.Add("git branch --remotes", "  origin/mob-session");

      var result = await fixture.Run("start");

      fakeCmdService.Commands.Should().NotContain("git branch -D mob-session");
    }

    [Fact]
    public async Task ShouldKeepUncommitedChangesWhenPrompted()
    {
      fakeCmdService.Add("git status --short", "work in progress");
      fakeSayService.GetYesNoAnswer = true;

      var result = await fixture.Run("start");

      fakeCmdService.Commands.Should().Contain("git stash");
      fakeCmdService.Commands.Should().Contain("git stash pop");
    }

    [Fact]
    public async Task ShouldDiscardUncommitedChangesBeforeStart()
    {
      fakeCmdService.Add("git status --short", "work in progress");
      fakeSayService.GetYesNoAnswer = false;

      var result = await fixture.Run("start");

      fakeCmdService.Commands.Should().Contain("git reset --hard");
      fakeCmdService.Commands.Should().NotContain("git stash");
      fakeCmdService.Commands.Should().NotContain("git stash pop");
    }

    [Fact]
    public async Task ShouldNotResetWhenNotingToCommit()
    {
      var result = await fixture.Run("start");

      fakeCmdService.Commands.Should().NotContain("git reset --hard");
    }
  }
}