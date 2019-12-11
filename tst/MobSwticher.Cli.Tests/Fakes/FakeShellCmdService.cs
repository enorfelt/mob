using MobSwitcher.Core.Services.Shell;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwticher.Cli.Tests.Fakes
{
    public class FakeShellCmdService : IShellCmdService
    {
        public FakeShellCmdService()
        {
            ShellCmdResponses = new Dictionary<string, string>();
        }

        public string Response { get; set; }

        public Dictionary<string, string> ShellCmdResponses { get; private set; }


        public string Run(string shellCmd)
        {
            var response = ShellCmdResponses[shellCmd];
            return response;
        }
    }
}
