using System.Collections.Generic;
using System.Linq;

namespace Ignite.Core.Settings
{
    public class SettingsContainer
    {
        public const string INCORRECT_OPTION = "incop";
        private List<Option> LoadedOptions = new List<Option>();

        public SettingsContainer() { }
        public SettingsContainer(List<Option> options)
        {
            LoadedOptions = options;
        }
        
        public void Write(string key, string val)
        {
            if(!LoadedOptions.Any((element) => element.Key == key))
            {
                LoadedOptions.Add(new Option(key, val));
            }
            else
            {
                var opt = LoadedOptions.First((element) => element.Key == key);
                opt.SetValue(val);

                LoadedOptions[LoadedOptions.FindIndex((element) => element.Key == key)] = opt;
            }
        }
        public string ReadValue(string key)
        {
            return Read(key)?.Value ?? INCORRECT_OPTION;
        }
        public Option Read(string key)
        {
            return LoadedOptions.FirstOrDefault((element) => element.Key == key) ?? new Option(INCORRECT_OPTION, INCORRECT_OPTION);
        }
        public List<Option> GetOptions() => LoadedOptions;
    }
}
