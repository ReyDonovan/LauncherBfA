using System;
using System.Runtime.InteropServices;

namespace Ignite.Core.Components.Launcher.External
{
    public struct StartupInfo
    {
        public uint Cb;
        public string Reserved;
        public string Desktop;
        public string Title;
        public uint X;
        public uint Y;
        public uint XSize;
        public uint YSize;
        public uint XCountChars;
        public uint YCountChars;
        public uint FillAttribute;
        public uint Flags;
        public short ShowWindow;
        public short Reserved2;
        public IntPtr ReservedHandle;
        public IntPtr StdInputHandle;
        public IntPtr StdOutputHandle;
        public IntPtr StdErrorHandle;

        public int Size => Marshal.SizeOf(typeof(StartupInfo));
    }
}
