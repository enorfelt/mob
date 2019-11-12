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

        public override string ToString()
        {
            var type = typeof(AppSettings);
            var properties = type.GetProperties();
            var result = string.Empty;
            foreach(var property in properties)
            {
                result += $"{property.Name} = {property.GetValue(this)}\n";
            }

            return result.Trim();
        }
    }
}
