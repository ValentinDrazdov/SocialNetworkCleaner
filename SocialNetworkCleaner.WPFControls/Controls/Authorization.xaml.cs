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

namespace SocialNetworkCleaner.WPFControls.Controls
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    /// 
    public delegate void AuthorizationSuccessDelegate(Connectors.VK.VKSession session);
    public partial class Authorization : UserControl
    {
        Connectors.VK.VKSession session;

        Connectors.VK.VKSession Session
        {
            get
            {
                return session;
            }
        }

        public event AuthorizationSuccessDelegate AuthorizationSuccess;

        public Authorization()
        {
            InitializeComponent();
        }

        private void auth_Click(object sender, RoutedEventArgs e)
        {
            session = new Connectors.VK.VKSession();


            var resp = session.Create(new CommonDataTypes.CreateSessionData()
            {
                Login = txtLogin.Text,
                Password = txtPass.Password,
                TwoFactorAuthorization = () =>
                {
                    return WPFControls.Windows.TwoFactorAuthorizationWindow.GetCode();
                }
            });

            if (resp.Type == CommonDataTypes.Enums.ResponseTypes.OK)
            {
                if (AuthorizationSuccess != null) AuthorizationSuccess(session);
                txtLogin.Foreground = Brushes.Green;
                lbLogin.Foreground = Brushes.Green;
                txtPass.Foreground = Brushes.Green;
                lbPassword.Foreground = Brushes.Green;
            }
            else
            {
                MessageBox.Show(resp.Message, resp.Type.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                txtLogin.Foreground = Brushes.Red;
                lbLogin.Foreground = Brushes.Red;
                txtPass.Foreground = Brushes.Red;
                lbPassword.Foreground = Brushes.Red;
            }
        }
    }
}
