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
    /// <summary>
    /// Interaction logic for RegisterScreen.xaml
    /// </summary>
    public partial class RegisterScreen : Window
    {
        public RegisterScreen()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                MessageBox.Show("All info are required, please kindly enter them all.\n Thank you!");
                return;
            }
            if (NativeFileManager.shared.IsFileExists("LoginData"))
            {
                String fileContentString = NativeFileManager.shared.ReadTextFromFile("LoginData");
                UserLoginDataContainer userLoginDataContainer = CoreDataManager.shared.ReadUserDataFromJsonString(fileContentString);
                foreach (var eachItem in userLoginDataContainer.data)
                {
                    if (eachItem.Username.ToLower() == UsernameTextBox.Text.ToLower())
                    {
                        MessageBox.Show("Username already existed.\nplease use another username.");
                        return;
                    }
                }
                UserLoginData newUser = new UserLoginData();
                newUser.Username = UsernameTextBox.Text;
                newUser.Email = EmailTextBox.Text;
                newUser.Password = PasswordTextBox.Password;
                newUser.UserUUID = Guid.NewGuid().ToString();
                userLoginDataContainer.data.Append(newUser);
                string jsonOutputContent = CoreDataManager.shared.WriteUserToJsonData(userLoginDataContainer);
                NativeFileManager.shared.SaveTextToFile(jsonOutputContent, "LoginData");
            }
            else
            {
                UserLoginDataContainer userLoginDataContainer = new UserLoginDataContainer();
                UserLoginData newUser = new UserLoginData();
                newUser.Username = UsernameTextBox.Text;
                newUser.Email = EmailTextBox.Text;
                newUser.Password = PasswordTextBox.Password;
                newUser.UserUUID = Guid.NewGuid().ToString();
                userLoginDataContainer.data.Append(newUser);
                string jsonOutputContent = CoreDataManager.shared.WriteUserToJsonData(userLoginDataContainer);
                NativeFileManager.shared.SaveTextToFile(jsonOutputContent, "LoginData");
            }
        }
    }
}
