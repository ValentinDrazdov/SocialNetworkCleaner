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

namespace SocialNetworkCleaner.WPFControls.Windows
{
    /// <summary>
    /// Логика взаимодействия для TwoFactorAuthorizationWindow.xaml
    /// </summary>
    public partial class TwoFactorAuthorizationWindow : Window
    {
        public string Code { get; set; }
        
        
        public TwoFactorAuthorizationWindow()
        {
            InitializeComponent();
        }

        private void btnTwoFactorCode_Click(object sender, RoutedEventArgs e)
        {
            Code = txtTwoFactorCode.Text;
            this.Close();
        }

        public static string GetCode()
        {
            TwoFactorAuthorizationWindow wnd = new TwoFactorAuthorizationWindow();
            wnd.ShowDialog();
            return wnd.Code;
        }
    }
}
