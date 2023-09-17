using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for DashboardScreen.xaml
    /// </summary>
    public partial class DashboardScreen : Window
    {
        UserWalletTransectionManager userWalletManager = UserWalletTransectionManager.shared;
        public UserGeneralInfoStruct userWallet;
        public List<TransectionPresentStruct> transectionPresent;
        const string loginHistory_sha256 = "857a3aaca61f85901deebacbd675f73f091c85ea52f835dc56ad77b4bae8fb28";
        public string user_token = "nil";

        public DashboardScreen()
        {
            InitializeComponent();
            user_token = NativeFileManager.shared.ReadTextFromFile(loginHistory_sha256);
            if (user_token == "nil")
            {
                MessageBox.Show("User Not Found");
                logoutAction();
            }
            DateCamboBox.Items.Add("Today");
            DateCamboBox.Items.Add("Yesterday");
            DateCamboBox.Items.Add("This Week");
            DateCamboBox.Items.Add("This Month");
            DateCamboBox.Items.Add("This Year");
            DateCamboBox.SelectedIndex = 0;
            refreshData();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddTransectionScreen addTransectionScreen = new AddTransectionScreen(userWallet);
            addTransectionScreen.theDashboard = this;
            addTransectionScreen.ShowDialog();
        }

        public void refreshData()
        {
            userWallet = userWalletManager.loadStructFromFile(user_token);

            List<TransectionStruct> tempTransetions = userWalletManager.getTransectionFilter(userWallet, dateComboBoxToEnum());
            transectionPresent = userWalletManager.transectionToPresent( userWallet, tempTransetions);
            TransectionTableView.ItemsSource = transectionPresent;
        }

        private PickDateEnum dateComboBoxToEnum()
        {
            switch(DateCamboBox.SelectedIndex)
            {
                case 0:
                    return PickDateEnum.today;
                case 1:
                    return PickDateEnum.yesterday;
                case 2:
                    return PickDateEnum.this_week;
                case 3:
                    return PickDateEnum.this_month;
                case 4:
                    return PickDateEnum.this_year;
                default:
                    return PickDateEnum.today;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "You are about to logout", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                logoutAction();
            }
        }

        private void logoutAction()
        {
            LoginScreen loginScreen = new LoginScreen();
            NativeFileManager.shared.deleteFile(loginHistory_sha256);
            loginScreen.Show();
            this.Close();
        }

        private void DateCamboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refreshData();
        }
    }
}
