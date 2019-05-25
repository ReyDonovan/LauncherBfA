using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ignite.Core.Language
{
    public class LanguageFileMgr
    {
        private const string DEFAULT_LANGUAGE_DIR = "locales";
        private const string DEFAULT_LANGUAGE_EXT = "pak";

        public LanguagePhrases ReadFile(Languages lang)
        {
            if(!Directory.Exists(DEFAULT_LANGUAGE_DIR))
            {
                Directory.CreateDirectory(DEFAULT_LANGUAGE_DIR);
            }

            if(!File.Exists($"{DEFAULT_LANGUAGE_DIR}\\{lang.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}"))
            {
                File.Create($"{DEFAULT_LANGUAGE_DIR}\\{lang.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}").Close();
                var def = Dictionaries.CreateByLanguage(lang);
                SaveFile(def);

                ReadFile(lang);
            }

            var lng = new LanguagePhrases();

            List<Phrase> phrases = new List<Phrase>();
            var temp = File.ReadAllLines($"{DEFAULT_LANGUAGE_DIR}\\{lang.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}");
            foreach (var line in temp)
            {
                var splitted = line.Split('=');
                phrases.Add(new Phrase(splitted[0], splitted[1]));
            }

            lng.AddDictionary(phrases);
            lng.SetLanguage(lang);

            return lng;
        }
        public void SaveFile(LanguagePhrases phrases)
        {
            if(!Directory.Exists(DEFAULT_LANGUAGE_DIR))
            {
                Directory.CreateDirectory(DEFAULT_LANGUAGE_DIR);
            }

            if (File.Exists($"{DEFAULT_LANGUAGE_DIR}\\{phrases.CurrentLanguage.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}"))
            {
                File.Delete($"{DEFAULT_LANGUAGE_DIR}\\{phrases.CurrentLanguage.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}");
                File.Create($"{DEFAULT_LANGUAGE_DIR}\\{phrases.CurrentLanguage.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}").Close();
            }

            using (var io = File.OpenWrite($"{DEFAULT_LANGUAGE_DIR}\\{phrases.CurrentLanguage.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}"))
            {
                foreach(var element in phrases.Phrases)
                {
                    byte[] tmp = Encoding.UTF8.GetBytes($"{element.Key}={element.Value}{Environment.NewLine}");
                    io.WriteAsync(tmp, 0, tmp.Length).Wait();
                }
            }
        }
    }
}
