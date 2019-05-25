using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Language
{
    public class LanguagePhrases
    {
        public Languages CurrentLanguage;
        public List<Phrase> Phrases { get; private set; } = new List<Phrase>();

        public void AddDictionary(List<Phrase> phrases) => Phrases = phrases;
        public void SetLanguage(Languages lang) => CurrentLanguage = lang;
        public Phrase GetPhrase(string key) => Phrases.FirstOrDefault((phrase) => phrase.Key == key);

        public string KeyOf(string key)
        {
            if (Phrases.Any((phrase) => phrase.Key == key))
            {
                return Phrases.First((phrase) => phrase.Key == key).Value;
            }
            else
            {
                return "unnamed-phrase-0";
            }
        }
        public List<string> GetKeys()
        {
            List<string> keys = new List<string>();

            Phrases.AsParallel().ForAll((element) =>
            {
                keys.Add(element.Key);
            });

            return keys;
        }
        public List<string> GetValues()
        {
            List<string> values = new List<string>();

            Phrases.AsParallel().ForAll((element) =>
            {
                values.Add(element.Value);
            });

            return values;
        }
    }
}
