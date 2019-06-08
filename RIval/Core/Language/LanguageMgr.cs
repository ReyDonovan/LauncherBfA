using Ignite.Core.Components;
using Ignite.Core.Language;
using System;

namespace Ignite.Core
{
    public enum Languages
    {
        Russian = 1,
        English = 2
    }

    public class LanguageMgr : Singleton<LanguageMgr>
    {
        private LanguagePhrases Phrases;
        private LanguageFileMgr FileMgr;
        private LangCfg Settings;

        public LanguageMgr()
        {
            FileMgr = new LanguageFileMgr();
            Phrases = FileMgr.ReadFile(FromConfig());

            Logger.Instance.WriteLine($"Language Mgr booted in: {DateTime.Now.ToFileTimeUtc()} lang: {FromConfig().ToString()}", LogLevel.Info);
        }

        public Languages GetCurrentLang()
        {
            return (Languages)Settings.LangKey;
        }

        public string GetLangShort()
        {
            return Settings.LangShort;
        }

        public Languages FromConfig()
        {
            Settings = CfgMgr.Instance.GetProvider().Read<LangCfg>(def: false);
            if(Settings == null)
            {
                Settings = CfgMgr.Instance.GetProvider().Read<LangCfg>(def: true);

                if(Settings == null)
                {
                    CfgMgr.Instance.GetProvider()
                        .Add(
                        new LangCfg()
                        {
                            LangShort = "ru-ru",
                            LangKey = (int)Languages.Russian
                        }, 
                        new LangCfg()
                        {
                            LangShort = "ru-ru",
                            LangKey = (int)Languages.Russian
                        })
                        .Build();

                    return FromConfig();
                }
            }

            return (Languages)Settings.LangKey;
        }
        public void SetLang(Languages lang)
        {
            if(lang == Languages.English)
            {
                CfgMgr.Instance.GetProvider().Write<LangCfg>((cfg) =>
                {
                    cfg.LangKey = (int)lang;
                    cfg.LangShort = "en-gb";
                }, 
                false);
            }
            else
            {
                CfgMgr.Instance.GetProvider().Write<LangCfg>((cfg) =>
                {
                    cfg.LangKey = (int)lang;
                    cfg.LangShort = "ru-ru";
                },
                false);
            }

            ApplicationEnv.Instance.Restart();
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
