using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwitcher.Core
{
    public class AppSettings
    {
        public bool UsePrettyPrint { get; set; }
        public string WipBranch { get; set; }
        public string RemoteName { get; set; }
        public string BaseBranch { get; set; }
        public string WipCommitMessage { get; set; }
    }
}
