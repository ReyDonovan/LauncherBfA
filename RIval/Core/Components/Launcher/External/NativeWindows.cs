using System;
using System.Runtime.InteropServices;

namespace Ignite.Core.Components.Launcher.External
{
    static class NativeWindows
    {
        /// kernel32.dll
        // Process
        [DllImport("kernel32.dll", EntryPoint = "CreateProcessA", SetLastError = true)]
        public static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref StartupInfo lpStartupInfo, out ProcessInformation lpProcessInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", EntryPoint = "TerminateProcess")]
        public static extern void TerminateProcess(IntPtr processHandle, int exitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandleA(string lpModuleName);

        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        // Thread
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenThread(ThreadAccessFlags dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SuspendThread(IntPtr hThread);

        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        // Memory
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpBaseAddress, out MemoryBasicInformation mbi, int dwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", EntryPoint = "FlushInstructionCache", SetLastError = true)]
        public static extern bool FlushInstructionCache(IntPtr hProcess, IntPtr lpBaseAddress, uint dwSize);

        /// ntdll.dll
        // Process
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern NtStatus NtQueryInformationProcess(IntPtr hProcess, int pic, ref ProcessBasicInformation pbi, int cb, out int pSize);

        // Page/View
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern NtStatus NtCreateSection(ref IntPtr sectionHandle, uint accessMask, IntPtr zero, ref LargeInteger maximumSize, uint protection, uint allocationAttributes, IntPtr zero2);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern NtStatus NtMapViewOfSection(IntPtr sectionHandle, IntPtr proccessHandle, ref IntPtr baseAddress, IntPtr zero, ulong regionSize, out LargeInteger sectionOffset, out uint viewSize, uint viewSection, IntPtr zero2, int protection);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern NtStatus NtUnmapViewOfSection(IntPtr processHandle, IntPtr baseAddress);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern IntPtr NtResumeProcess(IntPtr ProcessHandle);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern IntPtr NtSuspendProcess(IntPtr ProcessHandle);
    }
}
