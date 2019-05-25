using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.FileSystem
{
    public class FileCrashBuilder
    {

        private FileStream CurrentCrashLog;

        public FileCrashBuilder()
        {
            if (!Directory.Exists("crashes"))
            {
                Directory.CreateDirectory("crashes");
            }

            CurrentCrashLog = File.OpenWrite($"crashes\\{GetTimeStamp()}.report");
        }

        public FileCrashBuilder Append(string line, bool newLine = false)
        {
            if (newLine) line += Environment.NewLine;

            var buffer = line.ToByteArray();
            CurrentCrashLog.Write(buffer, 0, buffer.Count());

            return this;
        }
        public async Task<FileCrashBuilder> AppendAsync(string line, bool newLine = false)
        {
            if (newLine) line += Environment.NewLine;

            var buffer = line.ToByteArray();
            await CurrentCrashLog.WriteAsync(buffer, 0, buffer.Count());

            return this;
        }
        public void Save()
        {
            CurrentCrashLog.Close();
            CurrentCrashLog.Dispose();
        }

        private static string GetTimeStamp()
        {
            var stamp = DateTime.Now;

            return $"{stamp.Day}_{stamp.Month}_{stamp.Year}-{stamp.Hour}-{stamp.Minute}-{stamp.Second}__{stamp.Millisecond}";
        }
    }
}
