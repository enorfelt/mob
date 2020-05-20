using System;
using System.Collections.Generic;
using MobSwitcher.Core.Services.Shell;

namespace MobSwitcher.Cli.Tests.Fakes
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

    private Dictionary<string, string> ShellCmdResponses { get; set; }

    public void Add(string key, string value)
    {
      value = value.Replace("\r\n", Environment.NewLine);

      ShellCmdResponses.Add(key, value);
    }

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
