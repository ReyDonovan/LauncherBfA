using System;
using System.Runtime.InteropServices;

namespace Ignite.Core.Components.Launcher.External
{
    public struct ProcessInformation
    {
        public IntPtr ProcessHandle;
        public IntPtr ThreadHandle;
        public uint ProcessId;
        public uint ThreadId;

        public int Size => Marshal.SizeOf(typeof(ProcessInformation));
    }
}
