using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Language
{
    public class Phrase
    {
        public string Key   { get; private set; }
        public string Value { get; private set; }

        public Phrase() { }
        public Phrase(string key, string val)
        {
            Key   = key;
            Value = val;
        }
    }
}
