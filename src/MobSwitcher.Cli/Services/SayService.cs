using McMaster.Extensions.CommandLineUtils;
using MobSwitcher.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwitcher.Cli.Services
{
    public class SayService : ISayService
    {
        private readonly IConsole console;

        public SayService(IConsole console)
        {
            this.console = console;
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
