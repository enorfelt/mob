using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwitcher.Core.Services.Shell
{
    public interface IShellCmdService
    {
        string Run(string shellCmd);
    }
}
