using System.Runtime.InteropServices;

namespace Ignite.Core.Components.Launcher.External
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct LargeInteger
    {
        [FieldOffset(0)]
        public long Quad;
        [FieldOffset(0)]
        public uint Low;
        [FieldOffset(4)]
        public int High;

        public int Size => Marshal.SizeOf(typeof(LargeInteger));
    }
}
