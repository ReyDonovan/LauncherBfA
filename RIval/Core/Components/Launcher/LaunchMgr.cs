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
    public class LaunchMgr : Singleton<LaunchMgr>
    {
        public static Dictionary<string, Tuple<long, byte[]>> PatchList = new Dictionary<string, Tuple<long, byte[]>>();

        public async Task<bool> Launch(string path)
        {
            return await Task.Run(() =>
            {
                Store.Boot();

                return Start(Prepare(path + "\\Wow.exe", path));
            });
        }

        private string Prepare(string appPath, string folder)
        {
            if (!File.Exists(appPath))
            {
                Logger.Instance.WriteLine($"Executeable file not founded in path: '{appPath}'", LogLevel.Error);
            }

            // Check wow version.
            // Also allow 0 as workaround for the mac binary.
            if (Helpers.GetVersionValueFromClient(appPath, 0) != 30706 && Helpers.GetVersionValueFromClient(appPath, 0) != 0)
            {
                Logger.Instance.WriteLine($"Your client version {Helpers.GetVersionValueFromClient(appPath, 0)} is not supported.", LogLevel.Error);
            }

            var baseDirectory = Path.GetDirectoryName(folder);

            // Check and write the cert bundle.
            if (File.Exists($"{folder}/ca_bundle.txt.signed"))
            {
                var hash1 = BitConverter.ToString(SHA1.Create().ComputeHash(Convert.FromBase64String(Patches.Common.CertBundleData)));
                var hash2 = BitConverter.ToString(SHA1.Create().ComputeHash(File.ReadAllBytes($"{folder}/ca_bundle.txt.signed")));

                if (hash1 != hash2)
                    File.WriteAllBytes($"{folder}/ca_bundle.txt.signed", Convert.FromBase64String(Patches.Common.CertBundleData));
            }
            else
                File.WriteAllBytes($"{folder}/ca_bundle.txt.signed", Convert.FromBase64String(Patches.Common.CertBundleData));

            return appPath;
        }
        private bool Start(string appPath)
        {
            var startupInfo = new StartupInfo();
            var processInfo = new ProcessInformation();

            try
            {
                Logger.Instance.WriteLine($"Starting WoW ....", LogLevel.Debug);

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

                            Logger.Instance.WriteLine($"Waiting initialization ...", LogLevel.Debug);
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
                            Logger.Instance.WriteLine("Can't find all patterns.", LogLevel.Error);
                            Logger.Instance.WriteLine($"CertBundle: {certBundleOffset == 0}", LogLevel.Error);
                            Logger.Instance.WriteLine($"Signature: {signatureOffset == 0}", LogLevel.Error);
                        }

                        // Add the patches to the patch list.
                        PatchList.Add("CertBundle", Tuple.Create(certBundleOffset, Store.CertificatePatch));
                        PatchList.Add("Signature", Tuple.Create(signatureOffset, Store.SignaturePatch));

                        NativeWindows.NtResumeProcess(processInfo.ProcessHandle);

                        if (memory.RemapAndPatch(PatchList))
                        {
                            Logger.Instance.WriteLine($"Executeable successfully patched!", LogLevel.Debug);

                            binary = null;

                            return true;
                        }
                        else
                        {
                            binary = null;

                            Logger.Instance.WriteLine("Error while launching the client.", LogLevel.Error);

                            NativeWindows.TerminateProcess(processInfo.ProcessHandle, 0);

                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLine(ex.ToString(), LogLevel.Error);

                NativeWindows.TerminateProcess(processInfo.ProcessHandle, 0);

                return false;
            }

            return false;
        }
    }
}
