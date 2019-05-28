using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Language
{
    public class Dictionaries
    {
        public static LanguagePhrases CreateByLanguage(Languages lang)
        {
            if      (lang == Languages.English) return CreateEnglish();
            else if (lang == Languages.Russian) return CreateRussian();
            else return null;
        }
        public static LanguagePhrases CreateEnglish()
        {
            var lng = new LanguagePhrases();
            lng.AddDictionary(new List<Phrase>()
            {
                new Phrase("MagazineButtonHeader", "STORE"),
                new Phrase("ACPButtonHeader", "PRIVATE CABINET"),
                new Phrase("ForumButtonHeader", "FORUM"),
                new Phrase("ServersLabel", "SERVERS"),
                new Phrase("MiscLabel", "MISC"),
                new Phrase("LinksLabel", "LINKS"),
                new Phrase("SettingsButton", "SETTINGS"),
                new Phrase("Bugreport_Button", "BUGTRACKER"),
                new Phrase("StatusText_Ready", "Ready to play"),
                new Phrase("StatusText_ChooseGame", "Need choose the game folder"),
                new Phrase("StatusText_CheckFiles", "Check the files:"),
                new Phrase("StatusText_Download", "Dowload:"),
            });

            lng.SetLanguage(Languages.English);

            return lng;
        }
        public static LanguagePhrases CreateRussian()
        {
            var lng = new LanguagePhrases();
            lng.AddDictionary(new List<Phrase>()
            {
                new Phrase("MagazineButtonHeader", "МАГАЗИН"),
                new Phrase("ACPButtonHeader", "ЛИЧНЫЙ КАБИНЕТ"),
                new Phrase("ForumButtonHeader", "ФОРУМ"),
                new Phrase("ServersLabel", "СЕРВЕРА"),
                new Phrase("MiscLabel", "ДОПОЛНИТЕЛЬНО"),
                new Phrase("LinksLabel", "ССЫЛКИ"),
                new Phrase("SettingsButton", "НАСТРОЙКИ"),
                new Phrase("Bugreport_Button", "БАГТРЕКЕР"),
            });

            lng.SetLanguage(Languages.Russian);

            return lng;
        }
    }
}
