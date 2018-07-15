using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.CommonDataTypes
{
    public class CreateSessionData
    {
        private string _login;
        private string _password;
        private Func<string> _twoFactorAuthorization;

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public Func<string> TwoFactorAuthorization
        {
            get
            {
                return _twoFactorAuthorization;
            }
            set
            {
                _twoFactorAuthorization = value;
            }
        }
        

    }
}
