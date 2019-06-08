using System;
using System.Runtime.InteropServices;

namespace Ignite.Core.Components.Launcher.External
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryBasicInformation
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public MemProtection AllocationProtect;
        public IntPtr RegionSize;
        public MemState State;
        public MemProtection Protect;
        public MemType Type;

        public int Size => Marshal.SizeOf(typeof(MemoryBasicInformation));
    }
}
