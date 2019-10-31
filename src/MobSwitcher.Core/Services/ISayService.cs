using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwitcher.Core.Services
{
    public interface ISayService
    {
        void Say(string s);
        void SayError(string s);
        void SayOkay(string s);
        void SayNote(string s);
        void SayTodo(string s);
        void SayInfo(string s);
    }
}
