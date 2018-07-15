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

namespace SocialNetworkCleaner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<CommonDataTypes.SocialElements.Post> allposts;
        public MainWindow()
        {
            InitializeComponent();

            //Connectors.IProcessor processor = new Connectors.VK.VKProcessor();
            //allposts = processor.LoadData();
            //partPostsSearcher.AllPosts = allposts;
            
            //List< SocialNetworkCleaner.CommonDataTypes.SocialElements.IPostSource> sources = processor.KnownGroups.Cast<SocialNetworkCleaner.CommonDataTypes.SocialElements.IPostSource>().ToList();
            //sources.AddRange(processor.KnownUsers.Cast<SocialNetworkCleaner.CommonDataTypes.SocialElements.IPostSource>().ToList());

            //partPostsSearcher.AllSources = sources;
        }


        
        private void partAuth_AuthorizationSuccess(Connectors.VK.VKSession session)
        {
            partActions.IsAuth = true;
        }

        private void partActions_InformationChanged(List<CommonDataTypes.SocialElements.Post> posts, List<CommonDataTypes.SocialElements.IPostSource> sources)
        {
            
            partPosts.AllPosts = posts;
            partPosts.AllSources = sources;
            
        }

        private void partPosts_PostsSelected(List<CommonDataTypes.SocialElements.Post> posts)
        {
            partActions.SelectedPosts = posts;
        }

        private void partPosts_PostShowed(int count, int index)
        {
            partActions.StatusLabel = String.Format("Отображен пост: {0}/{1}", index, count);
        }
    }
}
