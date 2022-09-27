using Microsoft.Extensions.Options;
using MobSwitcher.Core.Services.Git;
using MobSwitcher.Core.Services.MobSwitch.Internal;
using System;

namespace MobSwitcher.Core.Services.MobSwitch
{
    public class MobSwitchService : IMobSwitchService
    {
        internal IGitService GitService { get; }

        internal ISayService Say { get; }

        internal IOptions<AppSettings> AppSettings { get; }

        public MobSwitchService(IGitService gitService, ISayService say, IOptions<AppSettings> appSettings)
        {
            this.GitService = gitService;
            this.Say = say;
            this.AppSettings = appSettings;
        }

        public void Done()
        {
            if (AppSettings.Value.UsePullRequest)
            {
                new MobSwitchDonePrCmd(this).Run();
            }
            else
            {
                new MobSwitchDoneCmd(this).Run();
            }
        }

        public void Join()
        {
            new MobSwitchJoinCmd(this).Run();
        }

        public void Next(bool stay = false)
        {
            new MobSwitchNextCmd(this).Run();
        }

        public void Reset()
        {
            new MobSwitchResetCmd(this).Run();
        }

        public void Start()
        {
            new MobSwitchStartCmd(this).Run();
        }

        public void Status()
        {
            new MobSwitchStatusCmd(this).Run();
        }
    }
}
