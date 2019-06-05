using Ignite.Core.Settings.Accessors;
using System;

namespace Ignite.Core.Settings
{
    public enum SettingsDriver
    {
        Json,
        Ini,
        File
    }

    public class SettingsProvider
    {
        public BaseAcessor       Accessor { get; private set; }
        public SettingsContainer Settings { get; private set; }

        public void Boot(SettingsDriver driver)
        {
            try
            {
                DispatchDriver(driver);
                Settings = Accessor.Load();
            }
            catch(NullReferenceException)
            {
                Components.Logger.Instance.WriteLine("Invalid settings boot accessor type.", Components.LogLevel.Error);
            }
        }
        public void Save() => Accessor.Save(Settings);

        private void DispatchDriver(SettingsDriver driver)
        {
            if (driver == SettingsDriver.Ini)
            {
                Accessor = new IniAccessor();
            }
            else if (driver == SettingsDriver.Json)
            {
                Accessor = new JsonAccessor();
            }
            else
            {
                Accessor = new BaseAcessor();
            }
        }
    }
}
