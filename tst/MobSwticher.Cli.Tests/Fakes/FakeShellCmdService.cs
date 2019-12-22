using MobSwitcher.Core.Services.Shell;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwticher.Cli.Tests.Fakes
{
    public class FakeShellCmdService : IShellCmdService
    {
        public int Called { get; private set; }

        public FakeShellCmdService()
        {
            ShellCmdResponses = new Dictionary<string, string>();
        }

        public Dictionary<string, string> ShellCmdResponses { get; private set; }


        public string Run(string shellCmd)
        {
            Called++;

            if (!ShellCmdResponses.ContainsKey(shellCmd))
                return string.Empty;

            var response = ShellCmdResponses[shellCmd];
            return response;
        }
    }
}
