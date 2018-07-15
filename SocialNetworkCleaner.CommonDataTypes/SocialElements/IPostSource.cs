using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.CommonDataTypes.SocialElements
{
    public interface IPostSource
    {
        string Guid { get; set; }
        long Uid { get; set; }
        int Id { get; set; }
        byte[] Picture { get; set; }
        string DisplayName { get; set; }
    }
}
