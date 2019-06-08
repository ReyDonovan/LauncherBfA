using System;
using System.Runtime.InteropServices;

namespace Ignite.Core.Components.Launcher.External
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ProcessBasicInformation
    {
        public IntPtr ExitStatus;
        public IntPtr PebBaseAddress;
        public IntPtr AffinityMask;
        public IntPtr BasePriority;
        public IntPtr UniqueProcessId;
        public IntPtr InheritedFromUniqueProcessId;

        public int Size => Marshal.SizeOf(typeof(ProcessBasicInformation));
    }
}
