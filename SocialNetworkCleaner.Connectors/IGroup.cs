using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors
{
    public interface IGroup
    {
        List<CommonDataTypes.SocialElements.Group> GetGroups(CommonDataTypes.SocialElements.User user=null);
        CommonDataTypes.SocialElements.Group GetGroup(string id);
    }
}
