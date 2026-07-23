using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WindowsAPICodePack.Dialogs;
using Revive.Services;
using Revive.Services.Launch;

namespace Revive.Pages
{
    public partial class Home : Page
    {
        private const string DllUrl = "https://raw.githubusercontent.com/cratoxgams/sadsaasas/refs/heads/main/Paradise.dll";
        private const string LaunchArgs = "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck";

        public Home()
        {
            InitializeComponent();
            LoadBuilds();
        }

        private void LoadBuilds()
        {
            List<Build> builds = new List<Build>();
            foreach (KeyValuePair<string, string> entry in UpdateINI.GetSection("Builds"))
            {
                builds.Add(new Build { Name = entry.Key, Path = entry.Value });
            }

            BuildBox.ItemsSource = builds;
            if (builds.Count > 0)
            {
                BuildBox.SelectedIndex = 0;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select a Fortnite build",
                Multiselect = false
            };

            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            if (!File.Exists(System.IO.Path.Combine(dialog.FileName, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe")))
            {
                MessageBox.Show("That folder does not contain a valid Fortnite build.");
                return;
            }

            string name = new DirectoryInfo(dialog.FileName).Name;
            UpdateINI.WriteToConfig("Builds", name, dialog.FileName);
            LoadBuilds();

            foreach (Build build in (List<Build>)BuildBox.ItemsSource)
            {
                if (build.Name == name)
                {
                    BuildBox.SelectedItem = build;
                    break;
                }
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (BuildBox.SelectedItem is Build selected)
            {
                UpdateINI.RemoveKey("Builds", selected.Name);
                LoadBuilds();
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (!(BuildBox.SelectedItem is Build selected))
            {
                MessageBox.Show("Add a build first.");
                return;
            }

            string path = selected.Path;
            if (!File.Exists(System.IO.Path.Combine(path, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe")))
            {
                MessageBox.Show("Fortnite build not found at that path.");
                return;
            }

            string email = UpdateINI.ReadValue("Auth", "Email");
            string password = UpdateINI.ReadValue("Auth", "Password");
            if (email == "NONE" || password == "NONE")
            {
                MessageBox.Show("No login saved. Sign in again.");
                return;
            }

            Task.Run(() => Launch(path, email, password));
        }

        private void Launch(string path, string email, string password)
        {
            try
            {
                string dllTarget = System.IO.Path.Combine(path, "Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64", "GFSDK_Aftermath_Lib.x64.dll");
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(dllTarget));

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(DllUrl, dllTarget);
                }

                PSBasics.Start(path, LaunchArgs, email, password);
                FakeAC.Start(path, "FortniteClient-Win64-Shipping_BE.exe", LaunchArgs, "r");
                FakeAC.Start(path, "FortniteLauncher.exe", LaunchArgs, "dsf");

                PSBasics._FortniteProcess.WaitForExit();

                FakeAC._FNLauncherProcess?.Close();
                FakeAC._FNAntiCheatProcess?.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to launch.");
            }
        }

        private class Build
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }
    }
}
