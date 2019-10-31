using System;
using System.Collections.Generic;
using System.Text;

namespace MobSwitcher.Core.Services
{
    public interface IToastService
    {
        void Toast(string message);
        void Toast(string message1, string message2);
    }
}
