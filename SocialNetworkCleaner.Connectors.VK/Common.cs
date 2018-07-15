using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors.VK
{
    internal class Common
    {
        static internal ulong vkAppId = -1;
        static private VkNet.VkApi _api;

        static public VkNet.VkApi Api
        {
            get
            {
                if (_api == null)
                {
                    _api = new VkNet.VkApi();
                }

                return _api;
            }
        }
    }
}
