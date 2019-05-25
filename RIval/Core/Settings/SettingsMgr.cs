using Ignite.Core.Components;
using Ignite.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core
{
    public class SettingsMgr : Singleton<SettingsMgr>
    {
        private SettingsProvider Provider { get; set; } = new SettingsProvider();

        public void Start(SettingsDriver driver)
        {
            Logger.Instance.WriteLine($"Settings Mgr booted in: {DateTime.Now.ToFileTimeUtc()} with driver: {driver.GetType().FullName}", LogLevel.Info);

            Provider.Boot(driver);
        }

        public void WriteValue(string key, string val) => Provider.Settings.Write(key, val);
        public void WriteOption(Option option) => Provider.Settings.Write(option.Key, option.Value);
        public string GetValue(string key) => Provider.Settings.ReadValue(key);
        public Option GetOption(string key) => Provider.Settings.Read(key);
        public SettingsContainer GetSettings() => Provider?.Settings ?? null;
        public void Save() => Provider?.Save();
    }
}
