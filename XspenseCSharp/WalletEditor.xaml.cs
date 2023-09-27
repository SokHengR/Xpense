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
    /// Interaction logic for WalletEditor.xaml
    /// </summary>
    public partial class WalletEditor : Window
    {
        bool isEditor = false;
        WalletStruct theWallet = new();
        UserWalletTransectionManager userManager = UserWalletTransectionManager.shared;
        WalletViewController walletVC;
        public WalletEditor(bool isEditor, WalletPresentStruct theWallet, WalletViewController walletVC)
        {
            InitializeComponent();
            UserGeneralInfoStruct originalStruct = userManager.loadStructFromFile(NativeFileManager.shared.GetUserToken());
            this.isEditor = isEditor;
            this.walletVC = walletVC;
            configTitle();

            // find and add currency --------
            foreach (CurrencyStruct eachCurrency in originalStruct.currency)
            {
                CurrencyComboBox.Items.Add(eachCurrency.full_name + " (" + eachCurrency.code_name + ")");
            }
            if (isEditor)
            {
                // find and apply theWallet
                foreach (WalletStruct eachWallet in originalStruct.wallet)
                {
                    if (eachWallet.uuid == theWallet.uuid)
                    {
                        this.theWallet = eachWallet;
                        break;
                    }
                }
                // apply name to input field --------
                WalletNameInput.Text = theWallet.Name;
                // find and apply category to [camboBox] --------
                for (int i = 0; i < originalStruct.currency.Count; i++)
                {
                    if (originalStruct.currency[i].uuid == theWallet.currency_id)
                    {
                        CurrencyComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        void configTitle()
        {
            if (isEditor)
            {
                Title = "Edit Wallet";
                TitleLabel.Content = "Edit Wallet";
                ApplyButton.Content = "Save Edit";
                return;
            }
            Title = "Create Wallet";
            TitleLabel.Content = "Create Wallet";
            ApplyButton.Content = "Create Wallet";
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(WalletNameInput.Text))
            {
                MessageBox.Show("Give your Wallet a name!");
                return;
            }
            if (CurrencyComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Give your Wallet a currency!");
                return;
            }

            UserGeneralInfoStruct originalStruct = userManager.loadStructFromFile(NativeFileManager.shared.GetUserToken());
            if (isEditor)
            {
                theWallet.name = WalletNameInput.Text;
                theWallet.currency_id = originalStruct.currency[CurrencyComboBox.SelectedIndex].uuid;
                userManager.editWallet(originalStruct, theWallet);
                walletVC.RefreshData();
                this.Close();
                return;
            }
            theWallet.uuid = Guid.NewGuid().ToString();
            theWallet.name = WalletNameInput.Text;
            theWallet.transection = new();
            theWallet.currency_id = originalStruct.currency[CurrencyComboBox.SelectedIndex].uuid;
            userManager.addWallet(originalStruct, theWallet);
            walletVC.RefreshData();
            this.Close();
        }
    }
}
