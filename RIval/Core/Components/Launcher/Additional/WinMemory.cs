using Ignite.Core.Components.Launcher.External;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ignite.Core.Components.Launcher.Additional
{
    public class WinMemory
    {
        public byte[] Data { get; private set; }

        public IntPtr ProcessHandle { get; }
        public IntPtr BaseAddress { get; }

        ProcessBasicInformation peb;

        public WinMemory(IntPtr processHandle)
        {
            ProcessHandle = processHandle;

            if (processHandle == IntPtr.Zero)
                throw new InvalidOperationException("No valid process found.");

            BaseAddress = ReadImageBaseFromPEB(processHandle);

            if (BaseAddress == IntPtr.Zero)
                throw new InvalidOperationException("Error while reading PEB data.");
        }

        public IntPtr Read(IntPtr address)
        {
            try
            {
                var buffer = new byte[IntPtr.Size];

                if (NativeWindows.ReadProcessMemory(ProcessHandle, address, buffer, buffer.Length, out var dummy))
                    return buffer.ToIntPtr();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return IntPtr.Zero;
        }

        public IntPtr Read(long address) => Read(new IntPtr(address));

        public byte[] Read(IntPtr address, int size)
        {
            try
            {
                var buffer = new byte[size];

                if (NativeWindows.ReadProcessMemory(ProcessHandle, address, buffer, size, out var dummy))
                    return buffer;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public byte[] Read(long address, int size) => Read(new IntPtr(address), size);

        public void Write(IntPtr address, byte[] data, MemProtection newProtection = MemProtection.ExecuteReadWrite)
        {
            try
            {
                NativeWindows.VirtualProtectEx(ProcessHandle, address, (uint)data.Length, (uint)newProtection, out var oldProtect);

                NativeWindows.WriteProcessMemory(ProcessHandle, address, data, data.Length, out var written);

                NativeWindows.FlushInstructionCache(ProcessHandle, address, (uint)data.Length);
                NativeWindows.VirtualProtectEx(ProcessHandle, address, (uint)data.Length, oldProtect, out oldProtect);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Write(long address, byte[] data, MemProtection newProtection = MemProtection.ExecuteReadWrite) => Write(new IntPtr(address), data, newProtection);

        public bool RemapAndPatch(IntPtr viewAddress, int viewSize, Dictionary<string, Tuple<long, byte[]>> patches)
        {
            // Suspend before remapping to prevent crashes.
            NativeWindows.NtSuspendProcess(ProcessHandle);

            Data = Read(viewAddress, viewSize);

            if (Data != null)
            {
                var newViewHandle = IntPtr.Zero;
                var maxSize = new LargeInteger { Quad = viewSize };

                if (NativeWindows.NtCreateSection(ref newViewHandle, 0xF001F, IntPtr.Zero, ref maxSize, 0x40u, 0x8000000, IntPtr.Zero) == NtStatus.Success &&
                    NativeWindows.NtUnmapViewOfSection(ProcessHandle, viewAddress) == NtStatus.Success)
                {
                    var viewBase = viewAddress;

                    // Map the view with original protections.
                    var result = NativeWindows.NtMapViewOfSection(newViewHandle, ProcessHandle, ref viewBase, IntPtr.Zero, (ulong)viewSize, out var viewOffset,
                                           out var newViewSize, 2, IntPtr.Zero, (int)MemProtection.ExecuteRead);

                    if (result == NtStatus.Success)
                    {
                        // Apply our patches.
                        foreach (var p in patches)
                        {
                            var address = p.Value.Item1;
                            var patch = p.Value.Item2;

                            // We are in a different section here.
                            if (address > Data.Length)
                            {
                                if (address < BaseAddress.ToInt64())
                                    address += BaseAddress.ToInt64();

                                Write(address, patch, MemProtection.ReadWrite);
                                continue;
                            }

                            for (var i = 0; i < patch.Length; i++)
                                Data[address + i] = patch[i];
                        }

                        var viewBase2 = IntPtr.Zero;

                        // Create a writable view to write our patches through to preserve the original protections.
                        result = NativeWindows.NtMapViewOfSection(newViewHandle, ProcessHandle, ref viewBase2, IntPtr.Zero, (uint)viewSize, out var viewOffset2,
                                               out var newViewSize2, 2, IntPtr.Zero, (int)MemProtection.ReadWrite);


                        if (result == NtStatus.Success)
                        {
                            // Write our patched data trough the writable view to the memory.
                            if (NativeWindows.WriteProcessMemory(ProcessHandle, viewBase2, Data, viewSize, out var dummy))
                            {
                                // Unmap them writeable view, it's not longer needed.
                                NativeWindows.NtUnmapViewOfSection(ProcessHandle, viewBase2);

                                var mbi = new MemoryBasicInformation();

                                // Check if the allocation protections is the right one.
                                if (NativeWindows.VirtualQueryEx(ProcessHandle, BaseAddress, out mbi, mbi.Size) != 0 && mbi.AllocationProtect == MemProtection.ExecuteRead)
                                {
                                    // Also check if we can change the page protection.
                                    if (!NativeWindows.VirtualProtectEx(ProcessHandle, BaseAddress, 0x4000, (uint)MemProtection.ReadWrite, out var oldProtect))
                                        NativeWindows.NtResumeProcess(ProcessHandle);

                                    return true;
                                }
                            }
                        }
                    }

                    Console.WriteLine("Error while mapping the view with the given protection.");
                }
            }
            else
                Console.WriteLine("Error while creating the view backup.");

            NativeWindows.NtResumeProcess(ProcessHandle);

            return false;
        }


        public bool RemapAndPatch(Dictionary<string, Tuple<long, byte[]>> patches)
        {

            var mbi = new MemoryBasicInformation();

            if (NativeWindows.VirtualQueryEx(ProcessHandle, BaseAddress, out mbi, mbi.Size) != 0)
                return RemapAndPatch(mbi.BaseAddress, mbi.RegionSize.ToInt32(), patches);

            return false;
        }

        /// Static functions.
        public static bool ResumeThread(int threadId)
        {
            var threadHandle = NativeWindows.OpenThread(ThreadAccessFlags.SuspendResume, false, (uint)threadId);

            if (threadHandle != IntPtr.Zero)
                return NativeWindows.ResumeThread(threadHandle) != -1;

            return false;
        }

        public static bool ResumeThreads(Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                if (!ResumeThread(thread.Id))
                    return false;
            }

            return true;
        }

        public static bool SuspendThread(int threadId)
        {
            var threadHandle = NativeWindows.OpenThread(ThreadAccessFlags.SuspendResume, false, (uint)threadId);

            if (threadHandle != IntPtr.Zero)
                return NativeWindows.SuspendThread(threadHandle) != -1;

            return false;
        }

        public static bool SuspendThreads(Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                if (!SuspendThread(thread.Id))
                    return false;
            }

            return true;
        }

        /// Private functions.
        IntPtr ReadImageBaseFromPEB(IntPtr processHandle)
        {
            try
            {
                if (NativeWindows.NtQueryInformationProcess(processHandle, 0, ref peb, peb.Size, out int sizeInfoReturned) == NtStatus.Success)
                    return Read(peb.PebBaseAddress.ToInt64() + 0x10);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return IntPtr.Zero;
        }
    }
}
