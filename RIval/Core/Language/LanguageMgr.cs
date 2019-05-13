using RIval.Core.Components;
using RIval.Core.Language;
using System;

namespace RIval.Core
{
    public enum Languages
    {
        Russian,
        English
    }

    public class LanguageMgr : Singleton<LanguageMgr>
    {
        private LanguagePhrases Phrases;
        private LanguageFileMgr FileMgr;

        public void Boot(Languages lang)
        {
            FileMgr = new LanguageFileMgr();
            Phrases = FileMgr.ReadFile(lang);

            Logger.Instance.WriteLine($"Language Mgr booted in: {DateTime.Now.ToFileTimeUtc()} lang: {lang.ToString()}", LogLevel.Info);
        }
        public Languages FromConfig()
        {
            Languages lang = Languages.English;

            foreach(var item in Enum.GetNames(typeof(Languages)))
            {
                if(item.ToLower() == SettingsMgr.Instance.GetValue("user-language"))
                {
                    lang = (Languages)Enum.Parse(typeof(Languages), item);
                }
            }

            return lang;
        }
        public string ValueOf(string key)
        {
            return Phrases.KeyOf(key);
        }
        public void Save()
        {
            FileMgr.SaveFile(Phrases);
        }
    }
}
