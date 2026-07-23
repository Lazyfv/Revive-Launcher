using System.Windows;
using Revive.Pages;

namespace Revive
{
    public partial class MainWindow : Window
    {
        private readonly Home home = new Home();
        private readonly Settings settings = new Settings();

        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Navigate(home);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(home);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(settings);
        }
    }
}
