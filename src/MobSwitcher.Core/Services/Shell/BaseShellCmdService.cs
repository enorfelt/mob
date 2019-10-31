using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MobSwitcher.Core.Services.Shell
{
    public abstract class BaseShellCmdService : IShellCmdService
    {
        public abstract string FileName { get; }

        public abstract string CmdParamName { get; }

        public string Run(string shellCmd)
        {
            var startInfo = new ProcessStartInfo(FileName);
            startInfo.Arguments = $"${CmdParamName} {shellCmd}";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.CreateNoWindow = true;

            using var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }
    }
}
