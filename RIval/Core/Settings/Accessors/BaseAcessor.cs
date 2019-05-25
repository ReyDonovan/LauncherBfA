using Ignite.Core.Settings.Contracts;
using System;
using System.IO;
using System.Text;

namespace Ignite.Core.Settings.Accessors
{
    public class BaseAcessor : ISettingsAccessor
    {
        private const string SETTINGS_DIR  = "settings";
        private const string SETTINGS_EXT  = "stg";
        private const string SETTINGS_NAME = "config";

        public virtual SettingsContainer CreateDefault()
        {
            return new SettingsContainer(new System.Collections.Generic.List<Option>()
            {
                new Option("gfx-shadows-enable", "1"),
                new Option("gfx-enable-anim", "1"),
                new Option("gfx-enable-smoothy", "1"),
                new Option("user-language", "russian"),
                new Option("version", ApplicationEnv.Instance.AppVersion.ToString())
            });
        }
        public virtual SettingsContainer Load()
        {
            if(!Directory.Exists(SETTINGS_DIR))
            {
                Directory.CreateDirectory(SETTINGS_DIR);
            }

            if(!File.Exists($"{SETTINGS_DIR}\\{SETTINGS_NAME}.{SETTINGS_EXT}"))
            {
                File.Create($"{SETTINGS_DIR}\\{SETTINGS_NAME}.{SETTINGS_EXT}").Close();
            }

            var lines = File.ReadAllLines($"{SETTINGS_DIR}\\{SETTINGS_NAME}.{SETTINGS_EXT}");
            if (lines == null || lines.Length <= 0)
            {
                Save(CreateDefault());
                lines = File.ReadAllLines($"{SETTINGS_DIR}\\{SETTINGS_NAME}.{SETTINGS_EXT}");
            }

            System.Collections.Generic.List<Option> opt = new System.Collections.Generic.List<Option>();

            foreach (var line in lines)
            {
                var splitted = line.Split('#');
                if (splitted.Length == 2)
                {
                    opt.Add(new Option(splitted[0], splitted[1]));
                }
            }

            return new SettingsContainer(opt);
        }
        public virtual void Save(SettingsContainer settings)
        {
            if (!Directory.Exists(SETTINGS_DIR))
            {
                Directory.CreateDirectory(SETTINGS_DIR);
            }

            if (File.Exists($"{SETTINGS_DIR}\\{SETTINGS_NAME}.{SETTINGS_EXT}"))
            {
                File.Delete($"{SETTINGS_DIR}\\{SETTINGS_NAME}.{SETTINGS_EXT}");
            }

            using (var io = File.Open($"{SETTINGS_DIR}\\{SETTINGS_NAME}.{SETTINGS_EXT}", FileMode.OpenOrCreate))
            {
                foreach(var opt in settings.GetOptions())
                {
                    byte[] tmp = Encoding.UTF8.GetBytes($"{opt.Key}#{opt.Value}{Environment.NewLine}");
                    io.WriteAsync(tmp, 0, tmp.Length).Wait();
                }
            }
        }
    }
}
