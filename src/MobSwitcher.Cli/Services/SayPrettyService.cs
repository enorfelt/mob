using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services;
using System;
using System.Text;

namespace MobSwitcher.Cli.Services
{
  public class SayPrettyService : ISayService
  {
    private readonly IConsole console;

    public SayPrettyService(IConsole console)
    {
      this.console = console;
      Console.OutputEncoding = Encoding.UTF8;
    }

    public bool GetYesNo(string message, bool defaultAnswer)
    {
      return Prompt.GetYesNo(message, defaultAnswer, ConsoleColor.DarkGreen);
    }

    public void Say(string s)
    {
      this.console.Out.WriteLine(s);
    }

    public void SayError(string s)
    {
      this.console.ForegroundColor = ConsoleColor.DarkYellow;
      this.console.Out.Write(" \x26A1 ");
      this.console.ResetColor();
      this.console.Out.WriteLine(s);
    }

    public void SayInfo(string s)
    {
      this.console.ForegroundColor = ConsoleColor.DarkCyan;
      this.console.Out.Write(" \xE602 ");
      this.console.ResetColor();
      this.console.Out.WriteLine(s);
    }

    public void SayNote(string s)
    {
      this.console.ForegroundColor = ConsoleColor.DarkRed;
      this.console.Out.Write(" \xF704 ");
      this.console.ResetColor();
      this.console.Out.WriteLine(s);
    }

    public void SayOkay(string s)
    {
      this.console.ForegroundColor = ConsoleColor.DarkGreen;
      this.console.Out.Write(" \xF62B ");
      this.console.ResetColor();
      this.console.Out.WriteLine(s);
    }

    public void SayTodo(string s)
    {
      this.console.ForegroundColor = ConsoleColor.DarkBlue;
      this.console.Out.Write(" \xF249 ");
      this.console.ResetColor();
      this.console.Out.WriteLine(s);
    }
  }
}
