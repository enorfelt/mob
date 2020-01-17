using FluentAssertions;
using MobSwticher.Cli.Tests.Fakes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MobSwticher.Cli.Tests
{
  public class NextCmdTests
  {
    protected readonly StartupFixture fixture;
    private readonly FakeShellCmdService fakeShellCmd;
    private readonly FakeSayService fakeSay;

    public NextCmdTests()
    {
      this.fixture = new StartupFixture();
      fakeShellCmd = this.fixture.FakeShellCmdService;
      fakeSay = this.fixture.FakeSayService;
    }

    [Theory]
    [InlineData("next")]
    [InlineData("NEXT")]
    [InlineData("n")]
    [InlineData("N")]
    public async Task ShouldNextOnArgument(string cmd)
    {
      var result = await fixture.Run(cmd);

      fakeShellCmd.Called.Should().BeGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task ShouldNotNextWhenNotMobbing()
    {
      fakeShellCmd.ShellCmdResponses.Add("git branch", string.Empty);

      await fixture.Run("next");

      fakeSay.Says.Last().Should().Contain("you aren't mobbing");
    }

    [Theory]
    [InlineData("User1\r\n", "User1\n", "User1")]
    [InlineData("User2\r\nUser1\r\n", "User2\n", "User1")]
    [InlineData("User1\r\nUser2\r\nUser1\r\n", "User1\n", "User2")]
    [InlineData("User1\r\nUser3\r\nUser2\r\nUser1\r\n", "User1\n", "User2")]
    [InlineData("User2\r\nUser1\r\nUser3\r\nUser2\r\nUser1\r\n", "User2\n", "User3")]
    public async Task NextShouldShowNameOfNextTypist(string commiters, string currentUsers, string expectedNext)
    {

      fixture.FakeShellCmdService = new FakeShellCmdService();
      fixture.FakeShellCmdService.ShellCmdResponses.Add("git rev-parse --is-inside-work-tree", "true");
      fixture.FakeShellCmdService.ShellCmdResponses.Add("git rev-parse --show-toplevel", "true");
      fixture.FakeShellCmdService.ShellCmdResponses.Add("git branch", "  master\n* mob-session\n");
      fixture.FakeShellCmdService.ShellCmdResponses.Add("git status --short", string.Empty);
      fixture.FakeShellCmdService.ShellCmdResponses.Add("git --no-pager log master..mob-session --pretty=\"format:%an\" --abbrev-commit", commiters);
      fixture.FakeShellCmdService.ShellCmdResponses.Add("git config --get user.name", currentUsers);
      fixture.FakeShellCmdService.ShellCmdResponses.Add("git checkout master", string.Empty);

      var result = await fixture.Run(new[] { "next" });

      result.Should().Be(0);
      fixture.FakeSayService.Says.Should().Contain($"***{expectedNext}*** is (probably) next.");
    }

    [Fact]
    public async Task NextShouldCheckoutMasterBranch()
    {
      fixture.FakeShellCmdService.ShellCmdResponses.Add("git branch", "  master\n* mob-session\n");

      var result = await fixture.Run(new[] { "next" });

      fixture.FakeShellCmdService.Commands.Last().Should().Contain("git checkout master");
    }

    [Theory]
    [InlineData("-s")]
    [InlineData("--Stay")]
    public async Task NextStayArgumentShouldStayOnMobSessionBranch(string arg)
    {
      fixture.FakeShellCmdService.ShellCmdResponses.Add("git branch", "  master\n* mob-session\n");

      var result = await fixture.Run(new[] { "next", arg });

      fixture.FakeShellCmdService.Commands.Last().Should().NotContain("git checkout master");
    }
  }
}