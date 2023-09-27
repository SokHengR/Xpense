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
    /// Interaction logic for CategoryEditor.xaml
    /// </summary>
    public partial class CategoryEditor : Window
    {
        bool isEditor = false;
        CategoryStruct theCategory = new();
        UserWalletTransectionManager userManager = UserWalletTransectionManager.shared;
        WalletViewController walletVC;

        public CategoryEditor(bool isEditor, CategoryStruct theCategory, WalletViewController walletVC)
        {
            InitializeComponent();
            this.isEditor = isEditor;
            this.theCategory = theCategory;
            this.walletVC = walletVC;
            configEditor();
        }
        void configEditor()
        {
            if (isEditor)
            {
                Title = "Edit Category";
                TitleLabel.Content = "Edit Category";
                ApplyButton.Content = "Save Edit";
                CategoryNameInput.Text = theCategory.name;
                DescriptionInput.Text = theCategory.description;
                return;
            }
            Title = "Create Category";
            TitleLabel.Content = "Create Category";
            ApplyButton.Content = "Create Category";
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(CategoryNameInput.Text) || string.IsNullOrEmpty(DescriptionInput.Text))
            {
                MessageBox.Show("Not enough info.");
                return;
            }

            UserGeneralInfoStruct originalStruct = userManager.loadStructFromFile(NativeFileManager.shared.GetUserToken());
            if (isEditor)
            {
                theCategory.name = CategoryNameInput.Text;
                theCategory.description = DescriptionInput.Text;
                userManager.editCategory(originalStruct, theCategory);
                walletVC.RefreshData();
                this.Close();
                return;
            }
            theCategory.name = CategoryNameInput.Text;
            theCategory.description = DescriptionInput.Text;
            theCategory.uuid = Guid.NewGuid().ToString();
            userManager.addCategory(originalStruct, theCategory);
            walletVC.RefreshData();
            this.Close();
        }
    }
}
