using MobSwitcher.Core.Services.Shell;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwitcher.Core.Services.Git
{
    public class GitService : IGitService
    {
        private readonly IShellCmdService cmdService;
        private readonly ISayService say;

        public GitService(IShellCmdService cmdService, ISayService say)
        {
            this.cmdService = cmdService;
            this.say = say;
        }

        public string Git(string args, bool silent = false)
        {
            if (string.IsNullOrEmpty(args))
                throw new ArgumentNullException(nameof(args));

            var command = $"git {args}";
            var result = this.cmdService.Run(command);

            if (!silent)
                say.SayOkay(command);

            return result;
        }
    }
}
