using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.CommonDataTypes.Enums
{
    public enum ResponseTypes
    {
        OK = 0,
        FAILWithoutErrorMessage = 1,
        FAILWithErrorMessage = 2,
        UnexceptedUnknownError = 3
    }
}
