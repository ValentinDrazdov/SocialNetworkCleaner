using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.CommonDataTypes.SocialElements
{
    public class User : Source, IPostSource
    {

        private string _name;
        private string _surname;

        public string Name { get { return _name; } set { _name = value; DisplayName = String.Format("{0} {1}", _name, _surname); } }

        public string SurName { get { return _surname; } set { _surname = value; DisplayName = String.Format("{0} {1}", _name, _surname); } }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string BirthDate { get; set; }
        public string Occupation { get; set; }
        public string NickName { get; set; }
        
    }
}
