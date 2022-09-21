
namespace MobSwitcher.CommandLine.Services;

public class SayService : ISayService
{
  public bool GetYesNo(string message, bool defaultAnswer)
  {
    return defaultAnswer; //Prompt.GetYesNo(message, defaultAnswer);
  }

  public void Say(string s)
  {
    Console.WriteLine(s);
  }

  public void SayError(string s)
  {
    Console.WriteLine($" ⚡ {s}");

  }

  public void SayInfo(string s)
  {
    Console.WriteLine($" > {s}");
  }

  public void SayNote(string s)
  {
    Console.WriteLine($" ❗ {s}");
  }

  public void SayOkay(string s)
  {
    Console.WriteLine($" ✓ {s}");
  }

  public void SayTodo(string s)
  {
    Console.WriteLine($" ☐ {s}");
  }
}
