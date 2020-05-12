using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services;

namespace MobSwitcher.Cli.Services
{
  public class SayService : ISayService
  {
    private readonly IConsole console;

    public SayService(IConsole console)
    {
      this.console = console;
    }

    public bool GetYesNo(string message, bool defaultAnswer)
    {
      return Prompt.GetYesNo(message, defaultAnswer);
    }

    public void Say(string s)
    {
      this.console.Out.WriteLine(s);
    }

    public void SayError(string s)
    {
      this.console.Out.WriteLine($" ⚡ {s}");

    }

    public void SayInfo(string s)
    {
      this.console.Out.WriteLine($" > {s}");
    }

    public void SayNote(string s)
    {
      this.console.Out.WriteLine($" ❗ {s}");
    }

    public void SayOkay(string s)
    {
      this.console.Out.WriteLine($" ✓ {s}");
    }

    public void SayTodo(string s)
    {
      this.console.Out.WriteLine($" ☐ {s}");
    }
  }
}
