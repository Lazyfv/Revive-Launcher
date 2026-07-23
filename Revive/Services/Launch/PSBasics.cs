using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Revive.Services.Launch
{
    public static class PSBasics
    {
        public static Process _FortniteProcess;

        public static void Start(string path, string args, string email, string password)
        {
            if (email == null || password == null)
            {
                MessageBox.Show("Missing login details.");
                return;
            }

            string exe = Path.Combine(path, "FortniteGame\\Binaries\\Win64\\", "FortniteClient-Win64-Shipping.exe");
            if (!File.Exists(exe))
            {
                return;
            }

            _FortniteProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exe,
                    Arguments = $"-AUTH_LOGIN={email} -AUTH_PASSWORD={password} -AUTH_TYPE=epic " + args
                },
                EnableRaisingEvents = true
            };

            _FortniteProcess.Exited += OnFortniteExit;
            _FortniteProcess.Start();
        }

        private static void OnFortniteExit(object sender, EventArgs e)
        {
            if (_FortniteProcess != null && _FortniteProcess.HasExited)
            {
                _FortniteProcess = null;
            }

            FakeAC._FNLauncherProcess?.Kill();
            FakeAC._FNAntiCheatProcess?.Kill();
        }
    }
}
