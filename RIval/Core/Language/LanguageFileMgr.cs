using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ignite.Core.Components.Message;

namespace Ignite.Core.Language
{
    public class LanguageFileMgr
    {
        private const string DEFAULT_LANGUAGE_DIR = "locales";
        private const string DEFAULT_LANGUAGE_EXT = "pak";

        public LanguagePhrases ReadFile(Languages lang)
        {
            try
            {
                if (!Directory.Exists(DEFAULT_LANGUAGE_DIR))
                {
                    Directory.CreateDirectory(DEFAULT_LANGUAGE_DIR);
                }

                if (!File.Exists($"{DEFAULT_LANGUAGE_DIR}\\{lang.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}"))
                {
                    File.Create($"{DEFAULT_LANGUAGE_DIR}\\{lang.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}").Close();
                    var def = Dictionaries.CreateByLanguage(lang);

                    SaveFile(def);
                    ReadFile(lang);
                }
                else
                {
                    List<Phrase> tphrases = new List<Phrase>();
                    var tmp = File.ReadAllLines($"{DEFAULT_LANGUAGE_DIR}\\{lang.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}");
                    foreach (var line in tmp)
                    {
                        var splitted = line.Split('=');
                        tphrases.Add(new Phrase(splitted[0], splitted[1]));
                    }

                    if(Dictionaries.CreateByLanguage(lang).Phrases.Count > tphrases.Count)
                    {
                        File.Delete($"{DEFAULT_LANGUAGE_DIR}\\{lang.ToString().ToLower()}.{DEFAULT_LANGUAGE_EXT}");

                        ReadFile(lang);
                    }
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
            catch(Exception ex)
            {
                MessageBoxMgr.Instance.ShowReportError(
                    "#03-1814",
                    "Error while reading language file. Delete the 'locales' folder and try again",
                    Components.Api.ApiFacade.Instance.GetUri("api-errorreporter-language"),
                    ex.ToString());

                return null;
            }
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
