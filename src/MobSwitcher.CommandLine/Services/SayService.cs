using MobSwitcher.Core.Services;
using System.CommandLine;
using System.CommandLine.IO;

namespace MobSwitcher.CommandLine.Services
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
            return defaultAnswer; //Prompt.GetYesNo(message, defaultAnswer);
        }

        public void Say(string s)
        {
            console.Out.WriteLine(s);
        }

        public void SayError(string s)
        {
            console.Out.WriteLine($" ⚡ {s}");

        }

        public void SayInfo(string s)
        {
            console.Out.WriteLine($" > {s}");
        }

        public void SayNote(string s)
        {
            console.Out.WriteLine($" ❗ {s}");
        }

        public void SayOkay(string s)
        {
            console.Out.WriteLine($" ✓ {s}");
        }

        public void SayTodo(string s)
        {
            console.Out.WriteLine($" ☐ {s}");
        }
    }
}
