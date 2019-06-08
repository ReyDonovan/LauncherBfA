using Ignite.Core.Components.Launcher.External;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Launcher.Additional
{
    public static class Helpers
    {
        public static BinaryTypes GetBinaryType(byte[] data)
        {
            BinaryTypes type = 0u;

            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                var magic = (uint)reader.ReadUInt16();

                // Check MS-DOS magic
                if (magic == 0x5A4D)
                {
                    reader.BaseStream.Seek(0x3C, SeekOrigin.Begin);

                    // Read PE start offset
                    var peOffset = reader.ReadUInt32();

                    reader.BaseStream.Seek(peOffset, SeekOrigin.Begin);

                    var peMagic = reader.ReadUInt32();

                    // Check PE magic
                    if (peMagic != 0x4550)
                        throw new NotSupportedException("Not a PE file!");

                    type = (BinaryTypes)reader.ReadUInt16();
                }
                else
                {
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);

                    type = (BinaryTypes)reader.ReadUInt32();
                }
            }

            return type;
        }

        public static int GetVersionValueFromClient(string fileName, byte field = 0)
        {
            var versInfo = FileVersionInfo.GetVersionInfo(fileName);

            var value = 0;

            switch (field)
            {
                case 1:
                    value = versInfo.FileBuildPart;
                    break;
                case 2:
                    value = versInfo.FileMinorPart;
                    break;
                case 3:
                    value = versInfo.FileMajorPart;
                    break;
                default:
                    value = versInfo.FilePrivatePart;
                    break;
            }

            return value;
        }

        public static string GetFileChecksum(byte[] data)
        {
            using (var stream = new BufferedStream(new MemoryStream(data), 1200000))
            {
                var sha256 = new SHA256Managed();
                var checksum = sha256.ComputeHash(stream);

                return BitConverter.ToString(checksum).Replace("-", "").ToLower();
            }
        }

        public static bool IsFileClosed(string filename)
        {
            try
            {
                using (var inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
