using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors
{
    public interface IPosts
    {
        event GetAllPostsNotificationEventHandler GetPostNotificationEvent;

        void DeletePost(long uid);
        List<CommonDataTypes.SocialElements.Post> FromUser(CommonDataTypes.SocialElements.User user, ulong count, ulong offset);
        CommonDataTypes.SocialElements.Post FromUserByID(CommonDataTypes.SocialElements.User user, CommonDataTypes.SocialElements.Post IDInform);
        List<CommonDataTypes.SocialElements.Post> FromGroup(CommonDataTypes.SocialElements.Group group, ulong count, ulong offset);
        CommonDataTypes.SocialElements.Post FromGroupByID(CommonDataTypes.SocialElements.Group group, CommonDataTypes.SocialElements.Post IDInform);
    }
}
