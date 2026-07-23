using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Revive.Services.Launch
{
    public static class FreezeExtensions
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);

        public static void Freeze(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                IntPtr handle = OpenThread(2, false, (uint)thread.Id);
                if (handle == IntPtr.Zero)
                {
                    break;
                }
                SuspendThread(handle);
            }
        }
    }
}
