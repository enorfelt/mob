using MobSwitcher.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwticher.Cli.Tests.Fakes
{
    public class FakeSayService : ISayService
    {
        public FakeSayService()
        {
            Infos = new List<string>();
        }

        public List<string> Infos { get; private set; }

        public void Say(string s)
        {
        }

        public void SayError(string s)
        {
        }

        public void SayInfo(string s)
        {
            Infos.Add(s);
        }

        public void SayNote(string s)
        {
        }

        public void SayOkay(string s)
        {
        }

        public void SayTodo(string s)
        {
        }
    }
}
