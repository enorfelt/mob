using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MobSwticher.Cli.Tests.Fakes;
using Xunit;

namespace MobSwticher.Cli.Tests {
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
      fakeCmdService.ShellCmdResponses.Add("git status --short", string.Empty);
      fakeCmdService.ShellCmdResponses.Add("git branch", "  mob-session");
      fakeCmdService.ShellCmdResponses.Add("git branch --remotes", "  origin/mob-session");

      var result = await fixture.Run("start");

      fakeSayService.Says.Should().Contain("rejoining mob session");
    }

    [Fact]
    public async Task ShouldCreateWhenSessionBranchDontExists() {
      fakeCmdService.ShellCmdResponses.Add("git status --short", string.Empty);
      fakeCmdService.ShellCmdResponses.Add("git branch", string.Empty);
      fakeCmdService.ShellCmdResponses.Add("git branch --remotes", string.Empty);

      var result = await fixture.Run("start");

      fakeSayService.Says.Any(r => r.Contains("create mob-session")).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldJoinWhenLocalSessionBranchDontExists() {
      fakeCmdService.ShellCmdResponses.Add("git status --short", string.Empty);
      fakeCmdService.ShellCmdResponses.Add("git branch", string.Empty);
      fakeCmdService.ShellCmdResponses.Add("git branch --remotes", "  origin/mob-session");

      var result = await fixture.Run("start");

      fakeSayService.Says.Any(r => r.Contains("joining mob session")).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldStartNewWhenLocalSessionBranchExistsAndRemoteDont() {
      fakeCmdService.ShellCmdResponses.Add("git status --short", string.Empty);
      fakeCmdService.ShellCmdResponses.Add("git branch", "  mob-session");
      fakeCmdService.ShellCmdResponses.Add("git branch --remotes", string.Empty);

      var result = await fixture.Run("start");

      fakeSayService.Says.Any(r => r.Contains("purging local branch and start new")).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldNotDeleteMobBranchWhenOnMobBranchAndMobbing()
    {
      fakeCmdService.ShellCmdResponses.Add("git status --short", string.Empty);
      fakeCmdService.ShellCmdResponses.Add("git branch", "* mob-session");
      fakeCmdService.ShellCmdResponses.Add("git branch --remotes", "  origin/mob-session");

      var result = await fixture.Run("start");

      fakeCmdService.Commands.Should().NotContain("git branch -D mob-session");
    }

    [Fact]
    public async Task ShouldKeepUncommitedChangesWhenPrompted()
    {
      fakeCmdService.ShellCmdResponses.Add("git status --short", "work in progress");
      fakeSayService.GetYesNoAnswer = true;

      var result = await fixture.Run("start");

      fakeCmdService.Commands.Should().Contain("git stash");
      fakeCmdService.Commands.Should().Contain("git stash pop");
    }

    [Fact]
    public async Task ShouldDiscardUncommitedChangesBeforeStart()
    {
      fakeCmdService.ShellCmdResponses.Add("git status --short", "work in progress");
      fakeSayService.GetYesNoAnswer = false;

      var result = await fixture.Run("start");

      fakeCmdService.Commands.Should().Contain("git reset --hard");
      fakeCmdService.Commands.Should().NotContain("git stash");
      fakeCmdService.Commands.Should().NotContain("git stash pop");
    }
  }
}