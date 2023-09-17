using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for AddTransectionScreen.xaml
    /// </summary>
    public partial class AddTransectionScreen : Window
    {
        public DashboardScreen theDashboard { get; set; }
        UserGeneralInfoStruct  userGeneralPublic;
        const string loginHistory_sha256 = "857a3aaca61f85901deebacbd675f73f091c85ea52f835dc56ad77b4bae8fb28";
        string user_token = "nil";

        public AddTransectionScreen(UserGeneralInfoStruct userGeneral)
        {
            InitializeComponent();
            userGeneralPublic = userGeneral;

            user_token = NativeFileManager.shared.ReadTextFromFile(loginHistory_sha256);

            TransectionDatePicker.SelectedDate = DateTime.Now;
            TypeComboBox.Items.Add("Expense");
            TypeComboBox.Items.Add("Income");
            foreach(WalletStruct eachWallet in userGeneral.wallet)
            {
                string currencyCodeName = "ERROR";
                foreach (CurrencyStruct eachCurrency in userGeneral.currency)
                {
                    if (eachCurrency.uuid == eachWallet.currency_id)
                    {
                        currencyCodeName = eachCurrency.code_name;
                    }
                }
                WalletComboBox.Items.Add(eachWallet.name + " (" + currencyCodeName + ")");
            }
            foreach (CategoryStruct eachCategory in userGeneral.category)
            {
                CategoryComboBox.Items.Add(eachCategory.name);
            }
        }

        private void AddTransectionButton_Click(object sender, RoutedEventArgs e)
        {
            float priceNumber;
            if (PricingInputField == null || TypeComboBox.SelectedIndex <= -1 || WalletComboBox.SelectedIndex <= -1)
            {
                MessageBox.Show("Not enough info to create transection");
                return;
            }
            if (!float.TryParse(PricingInputField.Text, out priceNumber))
            {
                MessageBox.Show("Invalid Price");
                return;
            }
            if (priceNumber <= 0)
            {
                MessageBox.Show("Price must be more than 0");
                return;
            }

            TransectionStruct newTransection = new TransectionStruct();
            newTransection.type = TypeComboBox.SelectedIndex == 0 ? TransectionTypeEnum.expense : TransectionTypeEnum.income;
            newTransection.uuid = Guid.NewGuid().ToString();
            newTransection.date = (DateTime)TransectionDatePicker.SelectedDate;
            newTransection.price = priceNumber;
            newTransection.category_id = userGeneralPublic.category[CategoryComboBox.SelectedIndex].uuid;
            newTransection.wallet_id = userGeneralPublic.wallet[WalletComboBox.SelectedIndex].uuid;

            userGeneralPublic.wallet[WalletComboBox.SelectedIndex].transection.Add(newTransection);

            UserWalletTransectionManager.shared.saveStructToFile(userGeneralPublic, user_token);
            theDashboard.refreshData();
            this.Close();
        }
    }
}
