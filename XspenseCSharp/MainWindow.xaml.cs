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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace XspenseCSharp
{
    public partial class MainWindow : Window
    {
        const string loginHistory_sha256 = "857a3aaca61f85901deebacbd675f73f091c85ea52f835dc56ad77b4bae8fb28";
        public MainWindow()
        {
            InitializeComponent();
            Invoke(EndThisScreen, 5000);
            Invoke(RisingProgressValue, 3000);
        }

        private static void Invoke(Action functionToRun, int millisecondsDelay)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(millisecondsDelay);
            timer.Tick += (sender, e) =>
            {
                timer.Stop();
                functionToRun();
            };
            timer.Start();
        }

        public void EndThisScreen()
        {
            if (NativeFileManager.shared.IsFileExists(loginHistory_sha256))
            {
                DashboardScreen dashboardScreen = new DashboardScreen();
                dashboardScreen.Show();
                this.Close();
            }
            else
            {
                LoginScreen loginScreen = new LoginScreen();
                loginScreen.Show();
                this.Close();
            }
        }

        void RisingProgressValue()
        {
            if(LoadingProgressBar.Value < 100)
            {
                LoadingProgressBar.Value += 1;
                Invoke(RisingProgressValue, 10);
            }
        }
    }
}
