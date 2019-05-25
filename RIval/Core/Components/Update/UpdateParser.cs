using System;
using IX.Composer.Architecture;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = IX.Composer.Architecture.Version;

namespace Ignite.Core.Components.Update
{
    public enum CompareResult
    {
        Invalid,
        Lower,
        Identity,
        Higher
    }
    public class UpdateParser
    {
        public static Version Parse(string ver)
        {
            if(Version.TryParse(ver, out var parsed))
            {
                return parsed;
            }
            else
            {
                return new Version("0.0.0.A");
            }
        }
        public static CompareResult Compare(Version v1, Version v2)
        {
            CompareResult res;

            if(v1 > v2)
            {
                res = CompareResult.Higher; 
            }
            else if(v1 == v2)
            {
                res = CompareResult.Identity;
            }
            else if(v1 < v2)
            {
                res = CompareResult.Lower;
            }
            else
            {
                res = CompareResult.Invalid;
            }

            return res;
        }
    }
}
