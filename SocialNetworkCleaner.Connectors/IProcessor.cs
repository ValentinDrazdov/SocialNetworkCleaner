using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors
{
    public delegate void GetAllPostsNotificationEventHandler(ulong count, ulong index, CommonDataTypes.SocialElements.Post post);
    public delegate void GetAllSourcesNotificationEventHandler(ulong count, ulong index, CommonDataTypes.SocialElements.IPostSource source);
    public delegate void AddInformationAboutSourceToPostsEventHandler(ulong count, ulong index, CommonDataTypes.SocialElements.Post post);
    public interface IProcessor
    {
        event GetAllPostsNotificationEventHandler GetPostsNotificationEvent;
        event GetAllSourcesNotificationEventHandler GetSourcesNotificationEvent;
        event AddInformationAboutSourceToPostsEventHandler AddInformationAboutSourceToPostsEvent;

        List<CommonDataTypes.SocialElements.Group> KnownGroups { get; }
        List<CommonDataTypes.SocialElements.User> KnownUsers { get; }
        List<CommonDataTypes.SocialElements.Post> GetAllPosts(CommonDataTypes.SocialElements.User user, long limit=0);
        List<CommonDataTypes.SocialElements.Group> GetAllUsersGroups(CommonDataTypes.SocialElements.User user);
        List<CommonDataTypes.SocialElements.User> GetAllUsersFriends(CommonDataTypes.SocialElements.User user);
        List<CommonDataTypes.SocialElements.Post> AddInformationAboutSourceToPosts(List<CommonDataTypes.SocialElements.Post> posts, CommonDataTypes.SocialElements.User currentUser, bool loadPictures);

        void DeletePost(long postUID);

        void SaveData(List<CommonDataTypes.SocialElements.Post> posts);
        List<CommonDataTypes.SocialElements.Post> LoadData();
    }
}
