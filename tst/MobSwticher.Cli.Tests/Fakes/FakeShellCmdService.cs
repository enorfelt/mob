using MobSwitcher.Core.Services.Shell;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwticher.Cli.Tests.Fakes
{
  public class FakeShellCmdService : IShellCmdService
  {
    public int Called { get; private set; }

    public List<string> Commands { get; set; }

    public List<string> Responses { get; set; }

    public FakeShellCmdService()
    {
      ShellCmdResponses = new Dictionary<string, string>();
      Commands = new List<string>();
      Responses = new List<string>();
    }

    public Dictionary<string, string> ShellCmdResponses { get; private set; }

    public string Run(string shellCmd)
    {
      Called++;

      Commands.Add(shellCmd);

      if (!ShellCmdResponses.ContainsKey(shellCmd))
        return string.Empty;

      var response = ShellCmdResponses[shellCmd];
      Responses.Add(response);
      return response;
    }
  }
}
