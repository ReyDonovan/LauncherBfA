using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIval.Core.Extensions
{
    public static class ByteArrayEx
    {
        public static int Count(this byte[] arr)
        {
            return arr.Length;
        }
    }
}
