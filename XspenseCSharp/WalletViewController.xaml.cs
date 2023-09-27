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
    /// Interaction logic for WalletViewController.xaml
    /// </summary>
    public partial class WalletViewController : Window
    {
        WalletViewTypeEnum viewingType = WalletViewTypeEnum.Wallet;
        List<WalletPresentStruct> walletList;
        List<CategoryStruct> CategoryList;
        List<CurrencyStruct> CurrencyList;
        UserWalletTransectionManager userManager = UserWalletTransectionManager.shared;

        public WalletViewController(WalletViewTypeEnum asType)
        {
            viewingType = asType;
            InitializeComponent();
            ConfigTitle();
            RefreshData();
            AttempHideColumn();
        }
        private void ConfigTitle()
        {
            switch (viewingType)
            {
                case WalletViewTypeEnum.Wallet:
                    Title = "Wallet";
                    TitleLabel.Content = "Wallet";
                    break;
                case WalletViewTypeEnum.Currency:
                    Title = "Currency";
                    TitleLabel.Content = "Currency";
                    break;
                case WalletViewTypeEnum.Category:
                    Title = "Category";
                    TitleLabel.Content = "Category";
                    break;
                default:
                    Title = "HEH?";
                    TitleLabel.Content = "HEH?";
                    break;
            }
        }
        public void RefreshData()
        {
            UserGeneralInfoStruct originalStruct = userManager.loadStructFromFile(NativeFileManager.shared.GetUserToken());
            switch (viewingType)
            {
                case WalletViewTypeEnum.Wallet:
                    walletList = userManager.WalletToWalletPresent(originalStruct);
                    WalletTableView.ItemsSource = walletList;
                    break;
                case WalletViewTypeEnum.Currency:
                    CurrencyList = originalStruct.currency;
                    WalletTableView.ItemsSource = CurrencyList;
                    break;
                case WalletViewTypeEnum.Category:
                    CategoryList = originalStruct.category;
                    WalletTableView.ItemsSource = CategoryList;
                    break;
                default:
                    break;
            }
            HideColumn();
        }
        private void AttempHideColumn()
        {
            if (WalletTableView.Columns.Count > 0)
            {
                HideColumn();
            }
            else
            {
                _ = Invoke(AttempHideColumn, 1);
            }
        }
        private void HideColumn()
        {
            switch (viewingType)
            {
                case WalletViewTypeEnum.Wallet:
                    if (WalletTableView.Columns.Count > 0)
                    {
                        DataGridColumn column0 = WalletTableView.Columns[0];
                        column0.Visibility = Visibility.Collapsed;
                        DataGridColumn column2 = WalletTableView.Columns[2];
                        column2.Visibility = Visibility.Collapsed;
                    }
                    break;
                case WalletViewTypeEnum.Currency:
                    if (WalletTableView.Columns.Count > 0)
                    {
                        DataGridColumn column0 = WalletTableView.Columns[0];
                        column0.Visibility = Visibility.Collapsed;
                    }
                    break;
                case WalletViewTypeEnum.Category:
                    if (WalletTableView.Columns.Count > 0)
                    {
                        DataGridColumn column0 = WalletTableView.Columns[0];
                        column0.Visibility = Visibility.Collapsed;
                    }
                    break;
                default:
                    break;
            }
        }
        public async Task Invoke(Action function, int timeInSeconds)
        {
            await Task.Delay(timeInSeconds * 100);
            function();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            UserGeneralInfoStruct originalStruct = userManager.loadStructFromFile(NativeFileManager.shared.GetUserToken());
            if (WalletTableView.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose an item to delete.");
                return;
            }
            switch (viewingType)
            {
                case WalletViewTypeEnum.Wallet:
                    MessageBoxResult result = MessageBox.Show("Delete this wallet?", "All transection in this wallet will be gone with it.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        userManager.deleteWallet(originalStruct, walletList[WalletTableView.SelectedIndex].uuid);
                    }
                    break;
                case WalletViewTypeEnum.Currency:
                    userManager.deleteCurrency(originalStruct, CurrencyList[WalletTableView.SelectedIndex]);
                    break;
                case WalletViewTypeEnum.Category:
                    userManager.deleteCategory(originalStruct, CategoryList[WalletTableView.SelectedIndex]);
                    break;
                default:
                    MessageBox.Show("HEH?");
                    break;
            }
            RefreshData();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            switch (viewingType)
            {
                case WalletViewTypeEnum.Wallet:
                    WalletEditor walletEdit = new(false, new(), this);
                    walletEdit.ShowDialog();
                    break;
                case WalletViewTypeEnum.Currency:
                    CurrencyEditor currencyEditor = new(false, new(), this);
                    currencyEditor.ShowDialog();
                    break;
                case WalletViewTypeEnum.Category:
                    CategoryEditor categoryEditor = new(false, new(), this);
                    categoryEditor.ShowDialog();
                    break;
                default:
                    MessageBox.Show("HEH?");
                    break;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (WalletTableView.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an item to edit");
                return;
            }
            switch (viewingType)
            {
                case WalletViewTypeEnum.Wallet:
                    WalletEditor walletEdit = new(true, walletList[WalletTableView.SelectedIndex], this);
                    walletEdit.ShowDialog();
                    break;
                case WalletViewTypeEnum.Currency:
                    CurrencyEditor currencyEditor = new(true, CurrencyList[WalletTableView.SelectedIndex], this);
                    currencyEditor.ShowDialog();
                    break;
                case WalletViewTypeEnum.Category:
                    CategoryEditor categoryEditor = new(true, CategoryList[WalletTableView.SelectedIndex], this);
                    categoryEditor.ShowDialog();
                    break;
                default:
                    MessageBox.Show("HEH?");
                    break;
            }
        }
    }
}
