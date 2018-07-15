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
using SocialNetworkCleaner.WPFControls.Helpers;

namespace SocialNetworkCleaner.WPFControls.Windows
{
    /// <summary>
    /// Логика взаимодействия для ImageViewerWindow.xaml
    /// </summary>
    public partial class ImageViewerWindow : Window
    {
        private CommonDataTypes.SocialElements.Post _data;
        public ImageViewerWindow()
        {
            InitializeComponent();
        }


        public CommonDataTypes.SocialElements.Post Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                
                picPostImage.LoadImageFromByteArray(_data.Picture);
                this.Topmost = true;
            }
        }

        private void picPostImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
