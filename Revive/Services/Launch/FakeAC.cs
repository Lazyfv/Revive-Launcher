using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Revive.Services.Launch
{
    public static class FakeAC
    {
        public static Process _FNLauncherProcess;
        public static Process _FNAntiCheatProcess;

        public static void Start(string path, string fileName, string args = "", string t = "r")
        {
            try
            {
                string exe = Path.Combine(path, "FortniteGame\\Binaries\\Win64\\", fileName);
                if (!File.Exists(exe))
                {
                    return;
                }

                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = exe,
                    Arguments = args,
                    CreateNoWindow = true
                };

                if (t == "r")
                {
                    _FNAntiCheatProcess = Process.Start(info);
                    _FNAntiCheatProcess.Freeze();
                }
                else
                {
                    _FNLauncherProcess = Process.Start(info);
                    _FNLauncherProcess.Freeze();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to start a process.");
            }
        }
    }
}
