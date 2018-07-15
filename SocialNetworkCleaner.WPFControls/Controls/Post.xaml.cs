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
using SocialNetworkCleaner.WPFControls.Helpers;

namespace SocialNetworkCleaner.WPFControls.Controls
{
    /// <summary>
    /// Логика взаимодействия для Post.xaml
    /// </summary>
    /// 
    public delegate void PostCheckedDelegate(bool status, CommonDataTypes.SocialElements.Post source);
    public partial class Post : UserControl
    {
        private CommonDataTypes.SocialElements.Post _data;
        public event PostCheckedDelegate Checked;

        public bool SelectionState
        {
            get
            {
                return partSource.SelectionState;
            }
            set
            {
                partSource.SelectionState = value;
            }
        }


        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
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
                _data.ImageLoadedEventHandler += ShowImage;
                _data.SelectionEventHandler += _data_SelectionEventHandler;
                if (_data.Source != null)
                {
                    partSource.Data = _data.Source;
                    partSource.SelectionTextFromData = _data;
                }
                else
                {
                    partSource.Visibility = Visibility.Collapsed;
                }
                
                lbPostText.Text = _data.Text;
                
            }
        }

        private void _data_SelectionEventHandler(bool selection)
        {
            partSource.selectionCheck.IsChecked = selection;
            if (Checked != null) Checked(selection, Data);
        }

        public void ShowImage()
        {
            if (_data.Picture != null)
            {
                picPostImage.LoadImageFromByteArray(_data.Picture);
            }
        }

        public Post()
        {
            InitializeComponent();
        }

        private void selectionCheck_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void partSource_SourceChecked(bool status, CommonDataTypes.SocialElements.IPostSource postSource)
        {
            if (Checked != null)
            {
                Checked(status, Data);
            }
        }

        private void picPostImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Windows.ImageViewerWindow imageViewerWindow = new Windows.ImageViewerWindow();
            imageViewerWindow.Data = this.Data;
            imageViewerWindow.Show();
        }
    }
}
