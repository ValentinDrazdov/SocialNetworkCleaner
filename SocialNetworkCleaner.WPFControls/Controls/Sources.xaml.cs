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
    /// Логика взаимодействия для Sources.xaml
    /// </summary>
    /// 

    public delegate void SourceFilterSourcesDelegate (List<CommonDataTypes.SocialElements.IPostSource> posts);
    public partial class Sources : UserControl
    {
        List<CommonDataTypes.SocialElements.IPostSource> filterSources;
        public event SourceFilterSourcesDelegate SourceFilterSources;
        List<SocialNetworkCleaner.CommonDataTypes.SocialElements.IPostSource> _sources;

        public List<SocialNetworkCleaner.CommonDataTypes.SocialElements.IPostSource> Data
        {
            get
            {
                return _sources;
            }
            set
            {
                _sources = value;
                if (_sources != null)
                {
                    var orderedSources = _sources.Where(x => x != null).OrderBy(x => x.DisplayName);
                    _sources = orderedSources.ToList();
                }
                foreach (var source in _sources)
                {

                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                    {
                        SourceHeader _source = new SourceHeader();
                        _source.Data = source;
                        _source.Width = 150;
                        _source.SourceChecked += _source_SourceChecked;
                        if (source.Uid < 0)
                        {
                            lstSourcesGroups.Children.Add(_source);
                        }
                        else
                        {
                            lstSourcesFriends.Children.Add(_source);
                        }
                    }));
                        
                    
                }
            }
        }

        public List<CommonDataTypes.SocialElements.IPostSource> FilterSources
        {
            get
            {

                if (filterSources == null)
                {
                    int cnt = 100;
                    if (Data != null)
                    {
                        cnt = Data.Count;
                    }
                    filterSources = new List<CommonDataTypes.SocialElements.IPostSource>(cnt);
                }
                return filterSources;
            }
        }
        

        private void _source_SourceChecked(bool status, CommonDataTypes.SocialElements.IPostSource postSource)
        {
            if (FilterSources.Contains(postSource))
            {
                if (!status) FilterSources.Remove(postSource);
            }
            else
            {
                if (status) FilterSources.Add(postSource);
            }

            if (SourceFilterSources != null) SourceFilterSources(FilterSources);
        }

        public Sources()
        {
            InitializeComponent();
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtFilter.Text;
            SortWrapPanel(filter, lstSourcesGroups);
            SortWrapPanel(filter, lstSourcesFriends);

        }

        private void SortWrapPanel (String filter, WrapPanel lstSource)
        {
            foreach (Object objSource in lstSource.Children)
            {
                SourceHeader source = objSource as SourceHeader;
                if (source == null) continue;
                if (String.IsNullOrEmpty(filter))
                {
                    source.Visibility = Visibility.Visible;
                }
                else
                {
                    if (source.Data.DisplayName.ToUpper().Contains(filter.ToUpper()) || source.SelectionState)
                    {
                        source.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        source.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }
}
