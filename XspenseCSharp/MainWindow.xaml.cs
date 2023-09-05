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
        public MainWindow()
        {
            InitializeComponent();
            Invoke(EndThisScreen, 5000);
            Invoke(RisingProgressValue, 50);
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
            this.Close();
        }

        void RisingProgressValue()
        {
            if(LoadingProgressBar.Value < 100)
            {
                LoadingProgressBar.Value += 1;
                Invoke(RisingProgressValue, 40);
            }
        }
    }
}
