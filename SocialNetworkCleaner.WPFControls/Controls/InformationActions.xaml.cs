using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для InformationActions.xaml
    /// </summary>
    /// 

    public delegate void InformationChangedDelegate(List<CommonDataTypes.SocialElements.Post> posts, List<CommonDataTypes.SocialElements.IPostSource> sources);
    public partial class InformationActions : UserControl
    {
        bool isAuth;
        bool isHaveInformation; bool loadImages;
        Connectors.IProcessor processor;
        List<CommonDataTypes.SocialElements.Post> posts;
        List<CommonDataTypes.SocialElements.Post> selectedPosts;
        public event InformationChangedDelegate InformationChanged;


        public InformationActions()
        {
            InitializeComponent();
        }

        public String StatusLabel
        {
            get
            {
                return txtStatus.Text;
            }
            set
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    txtStatus.Text = value;
                    this.UpdateLayout();
                }));                
            }
        }

        public bool IsAuth
        {
            get
            {
                return isAuth;
            }
            set
            {
                isAuth = value;
                loadFromVK.IsEnabled = isAuth;
                deleteSelected.IsEnabled = isAuth;
                
                StatusLabel = "Сменился статус авторизации => " + isAuth.ToString();
            }
        }

        public bool IsHaveInformation
        {
            get
            {
                return isHaveInformation;
            }
            set
            {
                if (InformationChanged != null)
                {

                    List<CommonDataTypes.SocialElements.IPostSource> sources = processor.KnownGroups.Cast<CommonDataTypes.SocialElements.IPostSource>().ToList();
                    sources.AddRange(processor.KnownUsers.Cast<CommonDataTypes.SocialElements.IPostSource>().ToList());

                    InformationChanged(posts, sources);

                    if (loadImages)
                    {
                        StatusLabel = String.Format("Получена информация. Постов: {0} Групп: {1}", posts.Count, sources.Count);
                        var postsWithPicture = posts.Where(x => x.Picture != null || !String.IsNullOrEmpty(x.PictureURLString));
                        if (postsWithPicture.Count() > 100)
                        {
                            var selected = MessageBox.Show("Было загружено более 100 изображений.\nОтображение всех изображений приведет к большой нагрузке на ваш компьютер и вызовет нежелательные временные зависания.\n\nВы уверены, что хотите сразу отобразить все изображения?\n\n*При отказе будет отображено лишь 100 изображений, что снизит нагрузку на компьютер.", "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                            if (selected == MessageBoxResult.No)
                            {
                                postsWithPicture = postsWithPicture.Take(100);
                            }
                        }
                        SelectedPosts.InsertRange(0, postsWithPicture.ToList());
                        showImages_Click(this, null);
                    }

                }

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    isHaveInformation = value;
                    saveToFS.IsEnabled = isHaveInformation;                    
                    showImages.IsEnabled = isHaveInformation;
                    loadFromVK.IsEnabled = true;
                }));
            }
        }

        public Connectors.IProcessor Processor
        {
            get
            {
                if (processor == null)
                {
                    Processor = new Connectors.VK.VKProcessor();
                    
                }
                return processor;
            }
            set
            {
                processor = value;
                processor.GetPostsNotificationEvent += Processor_GetPostsNotificationEvent;
                processor.GetSourcesNotificationEvent += Processor_GetSourcesNotificationEvent;
                processor.AddInformationAboutSourceToPostsEvent += Processor_AddInformationAboutSourceToPostsEvent;
            }
        }

        private void Processor_AddInformationAboutSourceToPostsEvent(ulong count, ulong index, CommonDataTypes.SocialElements.Post post)
        {
            StatusLabel = String.Format("Загружаем дополнительную информацию и связки с группами по посту ({0}/{1}), индекс текущего поста: {2}", index, count, post.Uid);
        }
        
        private void Processor_GetSourcesNotificationEvent(ulong count, ulong index, CommonDataTypes.SocialElements.IPostSource source)
        {
            if (source is SocialNetworkCleaner.CommonDataTypes.SocialElements.Group)
            {
                StatusLabel = String.Format("Загружаем группу ({0}/{1}), название группы: {2}", index, count, source.DisplayName);
            }
            if (source is SocialNetworkCleaner.CommonDataTypes.SocialElements.User)
            {
                StatusLabel = String.Format("Загружаем друга ({0}/{1}), Имя друга: {2}", index, count, source.DisplayName);
            }
        }

        private void Processor_GetPostsNotificationEvent(ulong count, ulong index, CommonDataTypes.SocialElements.Post post)
        {
            StatusLabel = String.Format("Загружаем блок постов ({0}/{1}), индекс текущего поста: {2}", index, count, post.Uid);
        }

        public List<CommonDataTypes.SocialElements.Post> Posts
        {
            get
            {
                if (posts == null) posts = new List<CommonDataTypes.SocialElements.Post>();
                return posts;
            }
            set
            {
                posts = value;
                if (posts != null && posts.Count > 0)
                {
                    IsHaveInformation = true;
                }
            }
        }

        public List<CommonDataTypes.SocialElements.Post> SelectedPosts
        {
            get
            {
                if (selectedPosts == null) selectedPosts = new List<CommonDataTypes.SocialElements.Post>(100);
                return selectedPosts;
            }
            set
            {
                selectedPosts = value;
            }
        }


        private void loadFromVK_Click(object sender, RoutedEventArgs e)
        {
            loadFromVK.IsEnabled = false;
            long limit = 0;
            if (!long.TryParse(txtLimit.Text, out limit))
            {
                limit = 0;
            }
            Task parallelLoad = new Task(new Action(() =>
            {
                var user = (new Connectors.VK.VKUser()).GetUserInfo();
                var _posts = Processor.GetAllPosts(user, limit);

                var groups = Processor.GetAllUsersGroups(user);
                var friends = Processor.GetAllUsersFriends(user);
                loadImages = false;
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    loadImages = checkLoadImages.IsChecked.Value;
                }));

                Posts = Processor.AddInformationAboutSourceToPosts(_posts, user, loadImages);
                

            }));
            parallelLoad.Start();
        }

        private void saveToFS_Click(object sender, RoutedEventArgs e)
        {
            Processor.SaveData(Posts);
        }

        private void loadFromFS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Posts = Processor.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка загрузки с диска. Возможно, отсутствуют требуемые файлы:\n\nusers.xml\ngroups.xml\nposts.xml", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void deleteSelected_Click(object sender, RoutedEventArgs e)
        {
            Task parallelDelete = new Task(new Action(() => {

                int postsCount = SelectedPosts.Count;
                int postNo = 0;
                foreach (var post in SelectedPosts)
                {
                    postNo++;
                    processor.DeletePost(post.Uid);
                    StatusLabel = String.Format("Удаляется пост: {0}/{1}", postNo, postsCount);

                    try
                    {
                        Posts.Remove(post);
                    }
                    catch (Exception ex)
                    {

                    }

                }
                IsHaveInformation = true;
                System.Threading.Thread.Sleep(200);
                var result = MessageBox.Show("В результате удаления информация в программе может не соответствовать реальной информации ВКонтакте. Для актуализации данных необходимо произвести повторную загрузку. Выполнить?", "Актуализация данных", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    StatusLabel = "Загружается информация из VK";
                    loadFromVK_Click(sender, null);
                }
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    saveToFS.IsEnabled = true;
                    loadFromFS.IsEnabled = true;
                    deleteSelected.IsEnabled = true;
                    loadFromVK.IsEnabled = true;
                    showImages.IsEnabled = true;
                }));                
            }));

            parallelDelete.Start();
            saveToFS.IsEnabled = false;
            loadFromFS.IsEnabled = false;
            deleteSelected.IsEnabled = false;
            loadFromVK.IsEnabled = false;
            showImages.IsEnabled = false;
        }

        private void txtLimit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void showImages_Click(object sender, RoutedEventArgs e)
        {
            Task parallelLoadImages = new Task(new Action(() =>
            {
                int postsCount = SelectedPosts.Count;
                int postNo = 0;
                List<SocialNetworkCleaner.CommonDataTypes.SocialElements.Post> postsToLoadPicture = new List<CommonDataTypes.SocialElements.Post>(SelectedPosts.Count);

                foreach (var post in SelectedPosts.ToArray())
                {
                    postNo++;
                    StatusLabel = String.Format("Загрузка изображения для поста: {0}/{1}", postNo, postsCount);

                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                    {
                        post.LoadPicture();
                        post.Selection(false);
                    }));
                }
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    saveToFS.IsEnabled = true;
                    loadFromFS.IsEnabled = true;
                    deleteSelected.IsEnabled = true;
                    loadFromVK.IsEnabled = true;
                    showImages.IsEnabled = true;
                    SelectedPosts.Clear();
                }));
            }));

            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                parallelLoadImages.Start();
                loadFromFS.IsEnabled = false;
                saveToFS.IsEnabled = false;
                deleteSelected.IsEnabled = false;
                loadFromVK.IsEnabled = false;
                showImages.IsEnabled = false;
            }));


        }
    }
}
