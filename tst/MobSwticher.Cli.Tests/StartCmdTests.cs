using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MobSwitcher.Cli.Tests.Fakes;
using Xunit;

namespace MobSwitcher.Cli.Tests {
  public class StartCmdTests {
    private StartupFixture fixture;

    public StartCmdTests() {
      fixture = new StartupFixture();
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
      fixture.FakeShellCmdService.Add("git status --short", string.Empty);
      fixture.FakeShellCmdService.Add("git branch", "  mob-session");
      fixture.FakeShellCmdService.Add("git branch --remotes", "  origin/mob-session");

      var result = await fixture.Run("start");

      fixture.FakeSayService.Says.Should().Contain("rejoining mob session");
    }

    [Fact]
    public async Task ShouldCreateWhenSessionBranchDontExists() {
      fixture.FakeShellCmdService.Add("git status --short", string.Empty);
      fixture.FakeShellCmdService.Add("git branch", string.Empty);
      fixture.FakeShellCmdService.Add("git branch --remotes", string.Empty);

      var result = await fixture.Run("start");

      fixture.FakeSayService.Says.Any(r => r.Contains("create mob-session")).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldJoinWhenLocalSessionBranchDontExists() {
      fixture.FakeShellCmdService.Add("git status --short", string.Empty);
      fixture.FakeShellCmdService.Add("git branch", string.Empty);
      fixture.FakeShellCmdService.Add("git branch --remotes", "  origin/mob-session");

      var result = await fixture.Run("start");

      fixture.FakeSayService.Says.Any(r => r.Contains("joining mob session")).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldStartNewWhenLocalSessionBranchExistsAndRemoteDont() {
      fixture.FakeShellCmdService.Add("git status --short", string.Empty);
      fixture.FakeShellCmdService.Add("git branch", "  mob-session");
      fixture.FakeShellCmdService.Add("git branch --remotes", string.Empty);

      var result = await fixture.Run("start");

      fixture.FakeSayService.Says.Any(r => r.Contains("purging local branch and start new")).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldNotDeleteMobBranchWhenOnMobBranchAndMobbing()
    {
      fixture.FakeShellCmdService.Add("git status --short", string.Empty);
      fixture.FakeShellCmdService.Add("git branch", "* mob-session");
      fixture.FakeShellCmdService.Add("git branch --remotes", "  origin/mob-session");

      var result = await fixture.Run("start");

      fixture.FakeShellCmdService.Commands.Should().NotContain("git branch -D mob-session");
    }

    [Fact]
    public async Task ShouldKeepUncommitedChangesWhenPrompted()
    {
      fixture.FakeShellCmdService.Add("git status --short", "work in progress");
      fixture.FakeSayService.GetYesNoAnswer = true;

      var result = await fixture.Run("start");

      fixture.FakeShellCmdService.Commands.Should().Contain("git stash");
      fixture.FakeShellCmdService.Commands.Should().Contain("git stash pop");
    }

    [Fact]
    public async Task ShouldDiscardUncommitedChangesBeforeStart()
    {
      fixture.FakeShellCmdService.Add("git status --short", "work in progress");
      fixture.FakeSayService.GetYesNoAnswer = false;

      var result = await fixture.Run("start");

      fixture.FakeShellCmdService.Commands.Should().Contain("git reset --hard");
      fixture.FakeShellCmdService.Commands.Should().NotContain("git stash");
      fixture.FakeShellCmdService.Commands.Should().NotContain("git stash pop");
    }

    [Fact]
    public async Task ShouldNotResetWhenNotingToCommit()
    {
      var result = await fixture.Run("start");

      fixture.FakeShellCmdService.Commands.Should().NotContain("git reset --hard");
    }
  }
}