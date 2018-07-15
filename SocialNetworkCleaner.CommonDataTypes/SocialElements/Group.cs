using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.CommonDataTypes.SocialElements
{
    public class Group : Source, IPostSource
    {
        private string _name;

        public string Name { get { return _name; } set { _name = value; DisplayName = _name; } }

        public bool IsModerator { get; set; }
        
    }
}
