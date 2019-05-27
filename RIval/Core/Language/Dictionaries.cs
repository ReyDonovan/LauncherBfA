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
            });

            lng.SetLanguage(Languages.Russian);

            return lng;
        }
    }
}
