using System;
using System.Diagnostics;

namespace Ignite.Core.Components.FileSystem
{
    public static class FileUtils
    {
        public static string FormatByte(long bytes)
        {
            int i;
            double dblSByte = bytes;
            for (i = 0; i < 5 && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, GetSuffix(i));
        }
        public static string GetSpeed(long bytes, Stopwatch sw)
        {
            int i;
            double dblSbyte = bytes;

            for (i = 0; i < 5 && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSbyte = bytes / 1024.0;
            }

            return $"{(dblSbyte / sw.Elapsed.TotalSeconds).ToString("0.00")} {GetSuffix(i)}/{LanguageMgr.Instance.ValueOf("Seconds_Short")}";
        }

        public static  string GetSuffix(int index)
        {
            string[] suffix =
            {
                "Bytes", "Kb", "Mb", "Gb", "Tb"
            };

            return LanguageMgr.Instance.ValueOf(suffix[index]);
        }
    }
}
