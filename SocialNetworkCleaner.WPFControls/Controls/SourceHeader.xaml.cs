using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для GroupHeader.xaml
    /// </summary>

    public delegate void SourceCheckedDelegate(bool status, CommonDataTypes.SocialElements.IPostSource postSource);

    public partial class SourceHeader : UserControl
    {
        public event SourceCheckedDelegate SourceChecked;
        private CommonDataTypes.SocialElements.IPostSource _data;
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
        public CommonDataTypes.SocialElements.IPostSource Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                if (_data == null)
                {
                    this.Visibility = Visibility.Collapsed;
                    return;
                }
                this.Visibility = Visibility.Visible;
                picGroupAvatar.LoadImageFromByteArray(_data.Picture);

                String uri;
                if (_data.Uid.ToString().Substring(0, 1) == "-")
                {
                    uri = "https://vk.com/public" + _data.Uid.ToString().Replace("-", "");
                }
                else
                {
                    uri = "https://vk.com/id" + _data.Uid;
                }
                Hyperlink hyperLink = new Hyperlink()
                {
                    NavigateUri = new Uri(uri)
                };
                hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
                hyperLink.Inlines.Add(_data.DisplayName);

                lbGroupName.Inlines.Clear();
                lbGroupName.Inlines.Add(hyperLink);

            }
        }


        public bool SelectionState
        {
            get
            {
                return selectionCheck.IsChecked.Value;
            }
            set
            {
                selectionCheck.IsChecked = value;
            }
        }

        public CommonDataTypes.SocialElements.Post SelectionTextFromData
        {
            get
            {
                return null;
            }
            set
            {
                TextBlock tb = new TextBlock();
                String uri;
                
                uri = String.Format("https://vk.com/wall{0}_{1}", value.OwnerID, value.Uid);
                Hyperlink hyperLink = new Hyperlink()
                {
                    NavigateUri = new Uri(uri)
                };
                hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
                hyperLink.Inlines.Add(value.Date);

                tb.Inlines.Clear();
                tb.Inlines.Add(hyperLink);
                selectionCheck.Content = tb;
            }
        }
        public string SelectionText
        {
            get
            {
                return selectionCheck.Content.ToString();
            }
            set
            {
                
                selectionCheck.Content = value;
            }
        }


        public SourceHeader()
        {
            InitializeComponent();
        }

        private void selectionCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (SourceChecked != null) SourceChecked(selectionCheck.IsChecked.Value, Data);
            if (selectionCheck.IsChecked.Value == true)
            {
                brdr.BorderThickness = new Thickness(3);
            }
            else
            {
                brdr.BorderThickness = new Thickness(0);
            }
            
        }
    }
}
