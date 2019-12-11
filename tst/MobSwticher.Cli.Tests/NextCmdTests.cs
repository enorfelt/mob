using FluentAssertions;
using MobSwticher.Cli.Tests.Fakes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MobSwticher.Cli.Tests
{
    public class NextCmdTests : IClassFixture<StartupFixture>
    {
        protected readonly StartupFixture fixture;

        public NextCmdTests(StartupFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory]
        [InlineData("Daniel\r\nMagnus\r\nErik\r\nDaniel\r\n", "Daniel\n", "Erik")]
        [InlineData("Erik\r\nDaniel\r\nMagnus\r\nErik\r\nDaniel\r\n", "Erik\n", "Magnus")]
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
            fixture.FakeSayService.Infos.Last().Should().Contain($"***{expectedNext}***");
            
        }
    }
}
