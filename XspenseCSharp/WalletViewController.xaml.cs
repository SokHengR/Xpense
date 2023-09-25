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
        UserWalletTransectionManager userManager = UserWalletTransectionManager.shared;

        public WalletViewController(WalletViewTypeEnum asType)
        {
            viewingType = asType;
            InitializeComponent();
            refreshData();
            AttempHideColumn();
        }
        public void refreshData()
        {
            switch (viewingType)
            {
                case WalletViewTypeEnum.Wallet:
                    UserGeneralInfoStruct originalStruct = userManager.loadStructFromFile(NativeFileManager.shared.GetUserToken());
                    walletList = userManager.WalletToWalletPresent(originalStruct);
                    WalletTableView.ItemsSource = walletList;
                    break;
                case WalletViewTypeEnum.Currency:
                    break;
                case WalletViewTypeEnum.Category:
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
                    break;
                case WalletViewTypeEnum.Category:
                    break;
                default:
                    break;
            }
        }
        public async Task Invoke(Action function, int timeInSeconds)
        {
            await Task.Delay(timeInSeconds * 1000);
            function();
        }
    }
}
