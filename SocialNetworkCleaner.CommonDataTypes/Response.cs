using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.CommonDataTypes
{
    public class Response
    {
        private Enums.ResponseTypes _type;
        private Enums.ResponseCodes _code;
        private string _message;

        public Enums.ResponseTypes Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public Enums.ResponseCodes Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }
        
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}\\[{1}]: {2}", Type, Code, Message);
        }
    }
}
