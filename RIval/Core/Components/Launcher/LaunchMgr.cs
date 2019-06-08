using Ignite.Core.Components.Launcher.Data;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Ignite.Core.Components.Launcher.Additional;
using Ignite.Core.Components.Launcher.External;
using System.Collections.Generic;

namespace Ignite.Core.Components.Launcher
{
    public class LaunchMgr
    {
        public static Dictionary<string, Tuple<long, byte[]>> PatchList = new Dictionary<string, Tuple<long, byte[]>>();

        public async void LaunchAndWait(string path)
        {
            await Task.Run(() =>
            {
                Store.Boot();

                Start(Prepare(path + "\\Wow.exe"));
            });
        }

        private string Prepare(string wowBinary)
        {
            // App info
            var curDir = AppDomain.CurrentDomain.BaseDirectory;
            var appPath = $"{curDir}/{wowBinary}";

            if (!File.Exists(appPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;

                //TODO:
                Console.WriteLine("Please copy the launcher to your WoW folder.");
            }

            // Check wow version.
            // Also allow 0 as workaround for the mac binary.
            if (Helpers.GetVersionValueFromClient(appPath, 0) != 28153 && Helpers.GetVersionValueFromClient(appPath, 0) != 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                //TODO:
                Console.WriteLine($"Your client version {Helpers.GetVersionValueFromClient(appPath, 0)} is not supported.");
            }

            var baseDirectory = Path.GetDirectoryName(curDir);

            // Check and write the cert bundle.
            if (File.Exists($"{curDir}/ca_bundle.txt.signed"))
            {
                var hash1 = BitConverter.ToString(SHA1.Create().ComputeHash(Convert.FromBase64String(Patches.Common.CertBundleData)));
                var hash2 = BitConverter.ToString(SHA1.Create().ComputeHash(File.ReadAllBytes($"{curDir}/ca_bundle.txt.signed")));

                if (hash1 != hash2)
                    File.WriteAllBytes($"{curDir}/ca_bundle.txt.signed", Convert.FromBase64String(Patches.Common.CertBundleData));
            }
            else
                File.WriteAllBytes($"{curDir}/ca_bundle.txt.signed", Convert.FromBase64String(Patches.Common.CertBundleData));

            return appPath;
        }
        private void Start(string appPath)
        {
            var startupInfo = new StartupInfo();
            var processInfo = new ProcessInformation();

            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Starting WoW client...");

                // Start process with suspend flags.
                if (NativeWindows.CreateProcess(null, $"{appPath}", IntPtr.Zero, IntPtr.Zero, false, 4U, IntPtr.Zero, null, ref startupInfo, out processInfo))
                {
                    var memory = new WinMemory(processInfo.ProcessHandle);
                    var modulusOffset = IntPtr.Zero;

                    // Get RSA modulus offset from file.
                    modulusOffset = File.ReadAllBytes(appPath).FindPattern(Patterns.Common.Modulus, memory.BaseAddress.ToInt64()).ToIntPtr();
                    var sectionOffset = memory.Read(modulusOffset, 0x2000).FindPattern(Patterns.Common.Modulus);

                    modulusOffset = (modulusOffset.ToInt64() + sectionOffset).ToIntPtr();

                    // Be sure that the modulus is written before the client is initialized.
                    while (memory.Read(modulusOffset, 1)[0] != Patches.Common.Modulus[0])
                        memory.Write(modulusOffset, Patches.Common.Modulus);

                    // Resume the process to initialize it.
                    NativeWindows.NtResumeProcess(processInfo.ProcessHandle);

                    var mbi = new MemoryBasicInformation();

                    // Wait for the memory region to be initialized.
                    while (NativeWindows.VirtualQueryEx(processInfo.ProcessHandle, memory.BaseAddress, out mbi, mbi.Size) == 0 || mbi.RegionSize.ToInt32() <= 0x1000)
                    { }

                    if (mbi.BaseAddress != IntPtr.Zero)
                    {
                        // Get the memory content.
                        var binary = new byte[0];

                        // Wait for client initialization.
                        var initOffset = memory?.Read(mbi.BaseAddress, mbi.RegionSize.ToInt32())?.FindPattern(Store.InitializePattern) ?? 0;

                        while (initOffset == 0)
                        {
                            initOffset = memory?.Read(mbi.BaseAddress, mbi.RegionSize.ToInt32())?.FindPattern(Store.InitializePattern) ?? 0;

                            Console.WriteLine("Waiting for client initialization...");
                        }

                        initOffset += BitConverter.ToUInt32(memory.Read((long)initOffset + memory.BaseAddress.ToInt64() + 2, 4), 0) + 10;

                        while (memory?.Read((long)initOffset + memory.BaseAddress.ToInt64(), 1)?[0] == null ||
                               memory?.Read((long)initOffset + memory.BaseAddress.ToInt64(), 1)?[0] == 0)
                            binary = memory.Read(mbi.BaseAddress, mbi.RegionSize.ToInt32());

                        // Suspend the process and handle the patches.
                        NativeWindows.NtSuspendProcess(processInfo.ProcessHandle);

                        //! Re-read the memory region for each pattern search.
                        var certBundleOffset = memory.Read(mbi.BaseAddress, mbi.RegionSize.ToInt32()).FindPattern(Store.CertificateBundle);
                        var signatureOffset = memory.Read(mbi.BaseAddress, mbi.RegionSize.ToInt32()).FindPattern(Store.SignatureBundle);

                        if (certBundleOffset == 0 || signatureOffset == 0)
                        {
                            Console.WriteLine("Can't find all patterns.");
                            Console.WriteLine($"CertBundle: {certBundleOffset == 0}");
                            Console.WriteLine($"Signature: {signatureOffset == 0}");
                        }

                        // Add the patches to the patch list.
                        PatchList.Add("CertBundle", Tuple.Create(certBundleOffset, Store.CertificatePatch));
                        PatchList.Add("Signature", Tuple.Create(signatureOffset, Store.SignaturePatch));

                        NativeWindows.NtResumeProcess(processInfo.ProcessHandle);

                        if (memory.RemapAndPatch(PatchList))
                        {
                            Console.WriteLine("Done :) ");

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("You can login now.");

                            binary = null;
                        }
                        else
                        {
                            binary = null;

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error while launching the client.");

                            NativeWindows.TerminateProcess(processInfo.ProcessHandle, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                NativeWindows.TerminateProcess(processInfo.ProcessHandle, 0);
            }
        }
    }
}
