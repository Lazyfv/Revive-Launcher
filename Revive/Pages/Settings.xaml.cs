using System.IO;
using System.Windows;
using System.Windows.Controls;
using WindowsAPICodePack.Dialogs;
using Revive.Services;

namespace Revive.Pages
{
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();

            string path = UpdateINI.ReadValue("Auth", "Path");
            if (path != "NONE" && path != null)
            {
                PathBox.Text = path;
            }
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select a Fortnite build",
                Multiselect = false
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (File.Exists(Path.Combine(dialog.FileName, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe")))
                {
                    PathBox.Text = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("That folder does not contain a valid Fortnite build.");
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            UpdateINI.WriteToConfig("Auth", "Path", PathBox.Text);
            MessageBox.Show("Saved.");
        }

        private void PathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateINI.WriteToConfig("Auth", "Path", PathBox.Text);
        }
    }
}
