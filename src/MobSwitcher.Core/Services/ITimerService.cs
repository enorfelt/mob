using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwitcher.Core.Services
{
    public interface ITimerService
    {
        void Start(int minutes);
    }
}
