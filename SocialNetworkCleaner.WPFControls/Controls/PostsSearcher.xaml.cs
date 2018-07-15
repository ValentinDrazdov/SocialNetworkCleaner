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
    /// Логика взаимодействия для PostsSearcher.xaml
    /// </summary>
    public partial class PostsSearcher : UserControl
    {
        public event PostShowedDelegate PostShowed;
        List<CommonDataTypes.SocialElements.Post> allposts;
        List<CommonDataTypes.SocialElements.IPostSource> allSources;
        List<CommonDataTypes.SocialElements.IPostSource> filterSources = new List<CommonDataTypes.SocialElements.IPostSource>();
        int filterCount = -1;

        public event PostsSelectedDelegate PostsSelected;

        public List<CommonDataTypes.SocialElements.Post> AllPosts
        {
            get
            {
                if (allposts == null) allposts = new List<CommonDataTypes.SocialElements.Post>();
                return allposts;
            }
            set
            {
                allposts = value;
                Show();
            }
        }
        public List<CommonDataTypes.SocialElements.IPostSource> AllSources
        {
            get
            {
                if (allSources == null) allSources = new List<CommonDataTypes.SocialElements.IPostSource>();
                return allSources;
            }
            set
            {
                allSources = value;
                componentSources.Data = allSources;
            }
        }

        public PostsSearcher()
        {
            InitializeComponent();
        }

        public void Show()
        {
            var filteredPosts = allposts.Where(x => filterSources.Count == 0 || (filterSources.Contains(x.Source)));
            if (filterCount > 0)
            {
                filteredPosts = filteredPosts.Take(filterCount);
            }
            componentPosts.Data = filteredPosts.ToList();
        }


        private void componentSources_SourceFilterSources(List<CommonDataTypes.SocialElements.IPostSource> filterSources)
        {
            this.filterSources = filterSources;
            Show();
        }

        private void componentPosts_PostsSelected(List<CommonDataTypes.SocialElements.Post> posts)
        {
            if (PostsSelected != null) PostsSelected(posts);
        }

        private void componentPosts_PostShowed(int count, int index)
        {
            if (PostShowed != null) PostShowed(count, index);
        }
    }
    

  

}
