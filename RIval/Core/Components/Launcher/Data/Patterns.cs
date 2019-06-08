using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Launcher.Data
{
    public static class Patterns
    {
        public static class Common
        {
            public static short[] Modulus = { 0x91, 0xD5, 0x9B, 0xB7, 0xD4, 0xE1, 0x83, 0xA5 };
        }

        public static class Retail
        {
            public static class Windows
            {
                public static short[] Init = { 0xC7, 0x05, -1, -1, -1, -1, 0x01, 0x00, 0x00, 0x00, 0x48, 0x8D, -1, -1, -1, -1, -1, 0x48, 0x8D };

                public static short[] CertBundle = { 0x45, 0x33, 0xC9, 0x4C, -1, -1, -1, -1, 0x48, -1, -1, -1, -1, 0x00, 0x00, 0x00, 0xE8, -1, -1, -1, -1, 0x84, 0xC0 };
                public static short[] Signature = { 0x0F, 0x82, -1, -1, -1, -1, 0x32, 0xC0, 0x48, 0x8B, -1, -1, -1, -1, -1, -1, 0x4C, 0x8D };
            }

            public static class Mac
            {
                public static short[] Init = { };

                public static short[] CertBundle = { };
                public static short[] Signature = { };
            }
        }
    }
}
