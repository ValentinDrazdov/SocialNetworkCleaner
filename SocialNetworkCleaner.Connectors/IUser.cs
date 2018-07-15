using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors
{
    public interface IUser
    {
        List<CommonDataTypes.SocialElements.User> GetUsers(CommonDataTypes.SocialElements.User user = null);
        CommonDataTypes.SocialElements.User GetUserInfo(CommonDataTypes.SocialElements.User idInfo);

    }
}
