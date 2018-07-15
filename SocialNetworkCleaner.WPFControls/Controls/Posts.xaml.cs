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
    /// Логика взаимодействия для Posts.xaml
    /// </summary>
    /// 
    public delegate void PostsSelectedDelegate(List<CommonDataTypes.SocialElements.Post> posts);
    public delegate void PostShowedDelegate(int count, int index);
    public partial class Posts : UserControl
    {
        List<CommonDataTypes.SocialElements.Post> _posts;
        List<CommonDataTypes.SocialElements.Post> selectedPosts;
        List<Post> loadedPosts;
        public event PostsSelectedDelegate PostsSelected;
        public event PostShowedDelegate PostShowed;

        public List<CommonDataTypes.SocialElements.Post> Data
        {
            get
            {
                return _posts;
            }
            set
            {
                if (value == null) return;
                if (loadedPosts == null)
                {
                    loadedPosts = new List<Post>(value.Count);
                }
                _posts = value;
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    wrapPosts.Children.Clear();
                }));
                int num = 0;
                int count = _posts.Count;
                foreach (var post in _posts)
                {

                    Post _post = null;
                    var searchposts = loadedPosts.Where(x => x.Data == post);
                    if (searchposts.Count() > 0)
                    {
                        _post = searchposts.First();
                    }
                    if (_post == null)
                    {
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                        {
                            _post = new Post();
                            _post.Data = post;
                            _post.Width = 260;
                            _post.Checked += _post_Checked;
                            loadedPosts.Add(_post);
                        }));
                    }
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                    {
                        wrapPosts.Children.Add(_post);
                    }));

                    num++;
                    if (PostShowed != null) PostShowed(count, num);
                }
            }
        }

        private void _post_Checked(bool status, CommonDataTypes.SocialElements.Post source)
        {
            if (SelectedPosts.Contains(source))
            {
                if (!status) SelectedPosts.Remove(source);
            }
            else
            {
                if (status) SelectedPosts.Add(source);
            }

            if (PostsSelected != null) PostsSelected(SelectedPosts);
        }

        public List<CommonDataTypes.SocialElements.Post> SelectedPosts
        {
            get
            {
                if (selectedPosts == null) selectedPosts = new List<CommonDataTypes.SocialElements.Post>();
                return selectedPosts;
            }
            set
            {
                selectedPosts = value;
            }
        }

        private void selectAllCheck_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Object objPost in wrapPosts.Children)
            {
                Post post = objPost as Post;
                if (post == null) continue;
                post.SelectionState = selectAllCheck.IsChecked.Value;
            }
        }
        //public void Filter(SocialNetworkCleaner.CommonDataTypes.SocialElements.Post _filter)
        //{

        //    var visibleItems = lstPosts.Items.Cast<Post>().Where(x => (_filter.IsRepost != null) ? _filter.IsRepost == x.Data.IsRepost : true ||
        //                                                              (_filter.SourceID != null) ? _filter.SourceID == x.Data.SourceID : true ||
        //                                                              (_filter.Text != null) ? x.Data.Text.Contains(_filter.Text) : true
        //    ).ToList();
        //    var hideItems = lstPosts.Items.Cast<Post>().Where(x => !visibleItems.Contains(x)).ToList();
        //    foreach (var item in visibleItems)
        //    {
        //        item.Visibility = Visibility.Visible;
        //    }
        //    foreach (var item in hideItems)
        //    {
        //        item.Visibility = Visibility.Collapsed;
        //    }

        //}

        public Posts()
        {
            InitializeComponent();
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtFilter.Text;
            foreach (Object objPost in wrapPosts.Children)
            {
                Post post = objPost as Post;
                if (post == null) continue;
                if (String.IsNullOrEmpty(filter))
                {
                    post.Visibility = Visibility.Visible;
                }
                else
                {
                    if (post.Data.Text.ToUpper().Contains(filter.ToUpper()))
                    {
                        post.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        post.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }
}
