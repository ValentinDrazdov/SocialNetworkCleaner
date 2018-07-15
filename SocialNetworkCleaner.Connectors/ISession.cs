using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors
{
    public interface ISession
    {
        bool IsActive { get; }
        CommonDataTypes.Response Create(CommonDataTypes.CreateSessionData createInfo);
    }
}
