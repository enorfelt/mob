using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwitcher.Core.Services.Shell
{
    public class CmdShellCmdService : BaseShellCmdService
    {
        public override string FileName => "cmd.exe";
        public override string CmdParamName => "/c";
        public override string EscapeCharacter => "^";
    }
}
