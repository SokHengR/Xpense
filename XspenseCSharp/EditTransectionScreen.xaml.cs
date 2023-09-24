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
    public partial class EditTransectionScreen : Window
    {
        public DashboardScreen theDashboard { get; set; }
        UserGeneralInfoStruct userGeneralPublic;
        const string loginHistory_sha256 = "857a3aaca61f85901deebacbd675f73f091c85ea52f835dc56ad77b4bae8fb28";
        string user_token = "nil";
        TransectionPresentStruct transectForEditPublic = new TransectionPresentStruct();
        UserWalletTransectionManager userWalletManager = UserWalletTransectionManager.shared;

        public EditTransectionScreen(UserGeneralInfoStruct userGeneral, TransectionPresentStruct transectionForEdit)
        {
            InitializeComponent();
            userGeneralPublic = userGeneral;

            user_token = NativeFileManager.shared.ReadTextFromFile(loginHistory_sha256);
            transectForEditPublic = transectionForEdit;

            PricingInputField.Text = transectionForEdit.Price.ToString();
            TransectionDatePicker.SelectedDate = transectionForEdit.Date;
            TypeComboBox.Items.Add("Expense");
            TypeComboBox.Items.Add("Income");
            TypeComboBox.SelectedIndex = transectionForEdit.Type == TransectionTypeEnum.expense ? 0 : 1;

            // find and add wallet --------
            foreach (WalletStruct eachWallet in userGeneral.wallet)
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
            // find and add category --------
            foreach (CategoryStruct eachCategory in userGeneral.category)
            {
                CategoryComboBox.Items.Add(eachCategory.name);
            }

            // find and apply wallet to [camboBox] --------
            for (int i = 0; i < userGeneral.wallet.Count; i++)
            {
                if (userGeneral.wallet[i].uuid == transectionForEdit.wallet_id)
                {
                    WalletComboBox.SelectedIndex = i;
                    break;
                }
            }
            // find and apply category to [camboBox] --------
            for (int i = 0; i < userGeneral.category.Count; i++)
            {
                if (userGeneral.category[i].uuid == transectionForEdit.category_id)
                {
                    CategoryComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void EditTransectionButton_Click(object sender, RoutedEventArgs e)
        {
            float priceNumber;
            if (PricingInputField == null || TypeComboBox.SelectedIndex <= -1 || WalletComboBox.SelectedIndex <= -1 || CategoryComboBox.SelectedIndex <= -1)
            {
                MessageBox.Show("Not enough info to create transection.");
                return;
            }
            if (!float.TryParse(PricingInputField.Text, out priceNumber))
            {
                MessageBox.Show("Invalid Price.\nPrice must be number.");
                return;
            }
            if (priceNumber <= 0)
            {
                MessageBox.Show("Invalid Price.\nPrice must be more than 0.");
                return;
            }

            transectForEditPublic.Type = TypeComboBox.SelectedIndex == 0 ? TransectionTypeEnum.expense : TransectionTypeEnum.income;
            transectForEditPublic.Date = (DateTime)TransectionDatePicker.SelectedDate;
            transectForEditPublic.Price = priceNumber;
            transectForEditPublic.category_id = userGeneralPublic.category[CategoryComboBox.SelectedIndex].uuid;
            transectForEditPublic.wallet_id = userGeneralPublic.wallet[WalletComboBox.SelectedIndex].uuid;

            userWalletManager.editTransection(userGeneralPublic, transectForEditPublic);

            theDashboard.refreshData();
            this.Close();
        }
    }
}
