using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIval.Core.Settings.Accessors
{
    public class JsonAccessor : BaseAcessor
    {
        public override void Save(SettingsContainer settings)
        {
            base.Save(settings);
        }

        public override SettingsContainer Load()
        {
            return base.Load();
        }
    }
}
