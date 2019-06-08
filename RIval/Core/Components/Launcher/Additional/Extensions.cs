using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Launcher.Additional
{
    public static class Extensions
    {
        public static IntPtr ToIntPtr(this byte[] buffer) => new IntPtr(BitConverter.ToInt64(buffer, 0));
        public static IntPtr ToIntPtr(this long value) => new IntPtr(value);

        public static long FindPattern(this byte[] data, short[] pattern, long start, long baseOffset = 0)
        {
            long matches;

            for (long i = start; i < data.Length; i++)
            {
                if (pattern.Length > (data.Length - i))
                    return 0;

                for (matches = 0; matches < pattern.Length; matches++)
                {
                    if ((pattern[matches] != -1) && (data[i + matches] != (byte)pattern[matches]))
                        break;
                }

                if (matches == pattern.Length)
                    return baseOffset + i;
            }

            return 0;
        }

        public static long FindPattern(this byte[] data, short[] pattern, long baseOffset = 0) => FindPattern(data, pattern, 0L, baseOffset);

        public static List<long> FindPattern(this byte[] data, short[] pattern, int maxMatches, long baseOffset = 0)
        {
            var matchList = new List<long>();

            long match = 0;

            do
            {
                match = data.FindPattern(pattern, match > baseOffset ? match - baseOffset : match, baseOffset);

                if (match != 0)
                {
                    if (!matchList.Contains(match))
                        matchList.Add(match);

                    match += pattern.Length;
                }

            } while (match != 0);

            return matchList;
        }
    }
}
