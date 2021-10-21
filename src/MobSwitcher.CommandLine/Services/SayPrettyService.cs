using MobSwitcher.Core.Services;
using System;
using System.CommandLine;
using System.CommandLine.IO;
using System.Text;

namespace MobSwitcher.CommandLine.Services
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
            return defaultAnswer;
            //return Prompt.GetYesNo(message, defaultAnswer, ConsoleColor.DarkGreen);
        }

        public void Say(string s)
        {
            console.Out.WriteLine(s);
        }

        public void SayError(string s)
        {
            //console.ForegroundColor = ConsoleColor.DarkYellow;
            console.Out.Write(" \x26A1 ");
            //console.ResetColor();
            console.Out.WriteLine(s);
        }

        public void SayInfo(string s)
        {
            //console.ForegroundColor = ConsoleColor.DarkCyan;
            console.Out.Write(" \xE602 ");
            //console.ResetColor();
            console.Out.WriteLine(s);
        }

        public void SayNote(string s)
        {
            //console.ForegroundColor = ConsoleColor.DarkRed;
            console.Out.Write(" \xF704 ");
            //console.ResetColor();
            console.Out.WriteLine(s);
        }

        public void SayOkay(string s)
        {
            //console.ForegroundColor = ConsoleColor.DarkGreen;
            console.Out.Write(" \xF62B ");
            //console.ResetColor();
            console.Out.WriteLine(s);
        }

        public void SayTodo(string s)
        {
            //console.ForegroundColor = ConsoleColor.DarkBlue;
            console.Out.Write(" \xF249 ");
            //console.ResetColor();
            console.Out.WriteLine(s);
        }
    }
}
