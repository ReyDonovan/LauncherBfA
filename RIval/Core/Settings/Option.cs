using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Settings
{
    public class Option
    {
        public string Key   { get; private set; }
        public string Value { get; private set; }

        public Option() { }
        public Option(string key, string val)
        {
            Key   = key;
            Value = val;
        }

        public void SetValue(string val)
        {
            Value = val;
        }
    }
}
