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
            if (NativeFileManager.shared.IsFileExists("LoginData"))
            {
                string userFileDataString = NativeFileManager.shared.ReadTextFromFile("LoginData");
                userLogin = CoreDataManager.shared.ReadUserDataFromJsonString(userFileDataString);
            }
            foreach (UserLoginData eachEle in userLogin.data)
            {
                if(eachEle.Username.ToLower() == UsernameInputField.Text.ToLower() && eachEle.Password == PasswordInputField.Password)
                {
                    MessageBox.Show(eachEle.UserUUID);
                    return;
                }
                MessageBox.Show("Incorrect password or username.\nIf you don't have an account please register.");
            }
        }
    }
}
