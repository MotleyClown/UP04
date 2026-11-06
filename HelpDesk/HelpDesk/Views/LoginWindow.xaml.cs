using System.Windows;
using HelpDesk.Services;

namespace HelpDesk.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml.
    /// Проверка учётных данных выполняется сервисом AuthService.
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordInput.Password;

            if (AuthService.Authenticate(login, password))
            {
                MainWindow mainWindow = new MainWindow();

                mainWindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show(
                    "Неверный логин или пароль.",
                    "Ошибка авторизации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}
