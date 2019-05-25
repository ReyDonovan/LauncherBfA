using Ignite.Core.Components;
using System;
using System.Collections;
using System.IO;

namespace Ignite.Core
{
    public static class ExceptionEx
    {
        public static void ToLog(this Exception ex, LogLevel level)
        {
            Logger.Instance.WriteLine(ex.Message, level);
        }

        public static void Report(this Exception ex, string crashLevel)
        {
            var crash = FileMgr.Instance.CreateCrash();
            crash.AppendAsync($"[Crash file created: {DateTime.Now.ToFileTime()}. Crash level: {crashLevel}]", true).Wait();
            crash.AppendAsync($"[Handled exception: Stack: {ex.StackTrace}, Message: {ex.Message}]", true).Wait();
            crash.AppendAsync($"[K-V Data]", true).Wait();
            crash.AppendAsync($"K-V START --", true).Wait();
            for(int i = 0; i < ex.Data.Count; i++)
            {
                crash.AppendAsync($" -- [{ex.Data[i]}]", true).Wait();
            }
            crash.AppendAsync($"K-V STOP --", true).Wait();
            crash.AppendAsync($"[Hardware information]", true).Wait();

            var os = ApplicationEnv.Instance.CurrentHardware.GetOS();
            crash.AppendAsync($" -- Operation system: Version: {os.GetOSVersion()}, Free memory: {os.GetOSFreePhysicalMemory()} bytes, Name: {os.GetOSName()}", true).Wait();

            var video = ApplicationEnv.Instance.CurrentHardware.GetVideo();
            crash.AppendAsync($" -- Video information: RAM: {video.GetVideoRAM()} bytes, Processor: {video.GetVideoProcessor()}, Caption: {video.GetVideoCaption()}", true).Wait();

            var socket = ApplicationEnv.Instance.CurrentHardware.GetProcessor();
            crash.AppendAsync($" -- Socket information: Name: {socket.GetProcessorName()}, Identifier: {socket.GetProcessorId()}", true).Wait();

            crash.Save();
        }
    }
}
