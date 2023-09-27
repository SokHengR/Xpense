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
    /// Interaction logic for CurrencyEditor.xaml
    /// </summary>
    public partial class CurrencyEditor : Window
    {
        bool isEditor = false;
        CurrencyStruct theCurrency = new();
        WalletViewController walletVC;
        UserWalletTransectionManager userManager = UserWalletTransectionManager.shared;

        public CurrencyEditor(bool isEditor, CurrencyStruct theCurrency, WalletViewController walletVC)
        {
            InitializeComponent();
            this.isEditor = isEditor;
            this.theCurrency = theCurrency;
            this.walletVC = walletVC;
            if (isEditor)
            {
                CurrencyNameInput.Text = theCurrency.full_name;
                ShortNameInput.Text = theCurrency.code_name.ToUpper();
                ExchangeRateInput.Text = theCurrency.exchange_rate.ToString();
            }
            configTitle();
        }
        void configTitle()
        {
            if(isEditor)
            {
                Title = "Edit Currency";
                TitleLabel.Content = "Edit Currency";
                ApplyButton.Content = "Save Edit";
            } else
            {
                Title = "Create Currency";
                TitleLabel.Content = "Create Currency";
                ApplyButton.Content = "Create Currency";
            }
        }
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            float exchangeRate = 0;
            if (string.IsNullOrEmpty(CurrencyNameInput.Text) || string.IsNullOrEmpty(ShortNameInput.Text))
            {
                MessageBox.Show("Not Enough Info.");
                return;
            }
            if (string.IsNullOrEmpty(ExchangeRateInput.Text))
            {
                MessageBox.Show("Exchange Rate is required");
                return;
            }
            if (!float.TryParse(ExchangeRateInput.Text, out exchangeRate))
            {
                MessageBox.Show("Invalid Exchange Rate.\nExchange Rate must be number.");
                return;
            }
            if(ShortNameInput.Text.Count() != 3)
            {
                MessageBox.Show("Short Name must be 3 Character long!\nExample: USD, KHR, JPY, VND ...");
                return;
            }
            if (exchangeRate <= 0)
            {
                MessageBox.Show("Invalid Price.\nPrice must be more than 0.");
                return;
            }

            UserGeneralInfoStruct originalStruct = userManager.loadStructFromFile(NativeFileManager.shared.GetUserToken());
            if (isEditor)
            {
                theCurrency.full_name = CurrencyNameInput.Text;
                theCurrency.code_name = ShortNameInput.Text.ToUpper();
                theCurrency.exchange_rate = exchangeRate;

                userManager.editCurrency(originalStruct, theCurrency);
                walletVC.RefreshData();
                this.Close();
                return;
            }
            theCurrency.uuid = Guid.NewGuid().ToString();
            theCurrency.full_name = CurrencyNameInput.Text;
            theCurrency.code_name = ShortNameInput.Text.ToUpper();
            theCurrency.exchange_rate = exchangeRate;

            userManager.addCurrency(originalStruct, theCurrency);
            walletVC.RefreshData();
            this.Close();
        }
    }
}
