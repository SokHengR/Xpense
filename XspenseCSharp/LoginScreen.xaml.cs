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
            if (NativeFileManager.shared.IsFileExists("LoginData"))
            {
                string userFileDataString = NativeFileManager.shared.ReadTextFromFile("LoginData");
                userLogin = CoreDataManager.shared.ReadUserDataFromJsonString(userFileDataString);
            }
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterScreen registerScreen = new RegisterScreen();
            registerScreen.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(userLogin.ToString());
        }
    }
}
