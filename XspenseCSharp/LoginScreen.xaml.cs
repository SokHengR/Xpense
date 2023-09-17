using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace XspenseCSharp
{
    public partial class LoginScreen : Window
    {
        UserLoginDataContainer userLogin;
        const string userAccount_sha256 = "375424f41e3db3a58eb56e7b78f2d99d3a91e7d3bcb9cea851f00369de51253a";
        const string loginHistory_sha256 = "857a3aaca61f85901deebacbd675f73f091c85ea52f835dc56ad77b4bae8fb28";
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterScreen registerScreen = new RegisterScreen();
            registerScreen.Show();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameInputField.Text) || string.IsNullOrEmpty(PasswordInputField.Password))
            {
                MessageBox.Show("Can't login with empty username or password");
                return;
            }
            if (NativeFileManager.shared.IsFileExists(userAccount_sha256))
            {
                string userFileDataString = NativeFileManager.shared.ReadTextFromFile(userAccount_sha256);
                userLogin = JsonConverterManager.shared.ReadUserDataFromJsonString(userFileDataString);
            }
            else
            {
                MessageBox.Show("Incorrect password or username.\nIf you don't have an account please register.");
                return;
            }
            foreach (UserLoginData eachEle in userLogin.data)
            {
                if (eachEle.Username.ToLower() == UsernameInputField.Text.ToLower() && eachEle.Password == PasswordInputField.Password)
                {
                    NativeFileManager.shared.SaveTextToFile(eachEle.UserUUID, loginHistory_sha256);
                    DashboardScreen dashboardScreen = new DashboardScreen();
                    this.Close();
                    return;
                }
                MessageBox.Show("Incorrect password or username.\nIf you don't have an account please register.");
            }
        }
    }
}
