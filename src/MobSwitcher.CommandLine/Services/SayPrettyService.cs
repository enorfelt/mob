namespace MobSwitcher.CommandLine.Services;
public class SayPrettyService : ISayService
{
  public SayPrettyService()
  {
    Console.OutputEncoding = Encoding.UTF8;
  }

  public bool GetYesNo(string message, bool defaultAnswer)
  {
    return defaultAnswer;
    // return Prompt.GetYesNo(message, defaultAnswer, ConsoleColor.DarkGreen);
  }

  public void Say(string s)
  {
    Console.WriteLine(s);
  }

  public void SayError(string s)
  {
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write(" \x26A1 ");
    Console.ResetColor();
    Console.WriteLine(s);
  }

  public void SayInfo(string s)
  {
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.Write(" \xE602 ");
    Console.ResetColor();
    Console.WriteLine(s);
  }

  public void SayNote(string s)
  {
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.Write(" \xF704 ");
    Console.ResetColor();
    Console.WriteLine(s);
  }

  public void SayOkay(string s)
  {
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.Write(" \xF62B ");
    Console.ResetColor();
    Console.WriteLine(s);
  }

  public void SayTodo(string s)
  {
    Console.ForegroundColor = ConsoleColor.DarkBlue;
    Console.Write(" \xF249 ");
    Console.ResetColor();
    Console.WriteLine(s);
  }
}
