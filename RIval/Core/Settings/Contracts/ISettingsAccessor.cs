using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Settings.Contracts
{
    public interface ISettingsAccessor
    {
        SettingsContainer CreateDefault();
        void Save(SettingsContainer settings);
        SettingsContainer Load();
    }
}
