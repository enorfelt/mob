using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwitcher.Core.Services.Git
{
    public interface IGitService
    {
        string Git(string args, bool silent = false);
    }
}
