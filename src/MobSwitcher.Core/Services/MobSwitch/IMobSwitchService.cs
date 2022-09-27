﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwitcher.Core.Services.MobSwitch
{
    public interface IMobSwitchService
    {
        void Reset();
        void Start();
        void Next(bool stay = false);
        void Done();
        void Status();
        void Join();
    }
}
