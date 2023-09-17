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
    /// Interaction logic for DashboardScreen.xaml
    /// </summary>
    public partial class DashboardScreen : Window
    {
        UserWalletTransectionManager userWalletManager = UserWalletTransectionManager.shared;
        UserGeneralInfoStruct userWallet;
        List<TransectionStruct> transectionData;
        const string loginHistory_sha256 = "857a3aaca61f85901deebacbd675f73f091c85ea52f835dc56ad77b4bae8fb28";
        public string user_token = "nil";

        public DashboardScreen()
        {
            InitializeComponent();
            user_token = NativeFileManager.shared.ReadTextFromFile(loginHistory_sha256);
            if (user_token == "nil")
            {
                MessageBox.Show("User Not Found");
                this.Close();
            }
            else
            {
                MessageBox.Show(user_token);
            }
            userWallet = userWalletManager.loadStructFromFile(user_token);
            transectionData = userWalletManager.getAllTransection(userWallet.wallet);
            TransectionTableView.ItemsSource = transectionData;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddTransectionScreen addTransectionScreen = new AddTransectionScreen();
            addTransectionScreen.Show();
        }
    }
}
