using System.Collections.Generic;
using MobSwitcher.Core.Services;

namespace MobSwitcher.Cli.Tests.Fakes
{
  public class FakeSayService : ISayService
  {
    public FakeSayService()
    {
      Says = new List<string>();
    }

    public List<string> Says { get; private set; }
    public bool GetYesNoAnswer { get; set; }

    public bool GetYesNo(string message, bool defaultAnswer)
    {
      return GetYesNoAnswer;
    }

    public void Say(string s)
    {
      Says.Add(s);
    }

    public void SayError(string s)
    {
      Says.Add(s);
    }

    public void SayInfo(string s)
    {
      Says.Add(s);
    }

    public void SayNote(string s)
    {
      Says.Add(s);
    }

    public void SayOkay(string s)
    {
      Says.Add(s);
    }

    public void SayTodo(string s)
    {
      Says.Add(s);
    }
  }
}
