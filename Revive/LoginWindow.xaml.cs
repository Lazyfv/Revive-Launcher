using System.Windows;
using Revive.Services;

namespace Revive
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            string email = UpdateINI.ReadValue("Auth", "Email");
            if (email != "NONE" && email != null)
            {
                EmailBox.Text = email;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Enter an email and password.");
                return;
            }

            UpdateINI.WriteToConfig("Auth", "Email", EmailBox.Text);
            UpdateINI.WriteToConfig("Auth", "Password", PasswordBox.Password);

            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
