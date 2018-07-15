using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors.VK
{
    public class VKSession : ISession
    {
        private string _token;
        CommonDataTypes.CreateSessionData loginInfo; 

        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
            }
        }
       
        public bool IsActive
        {
            get
            {
                return Common.Api.IsAuthorized;
            }
        }

        public CommonDataTypes.CreateSessionData LoginInfo
        {
            get
            {
                if (loginInfo == null) loginInfo = new CommonDataTypes.CreateSessionData();
                return loginInfo;
            }
        }


        public CommonDataTypes.Response Create(CommonDataTypes.CreateSessionData createInfo)
        {
            loginInfo = createInfo;
            CommonDataTypes.Response resp = new CommonDataTypes.Response();
            resp.Code = CommonDataTypes.Enums.ResponseCodes.Authorization;
            try
            {
          //      Token = (String) Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\DRAZDOV\\SOCIALNETWORKCLEANER\\", "LASTTOKEN", "");

                if (Token == "") Token = null;

                try
                {
                    Common.Api.Authorize(new VkNet.ApiAuthParams()
                    {
                        ApplicationId = Common.vkAppId,
                        Login = createInfo.Login,
                        Password = createInfo.Password,
                        Settings = VkNet.Enums.Filters.Settings.All,
                        TwoFactorAuthorization = createInfo.TwoFactorAuthorization
                    });
                }
                catch (Exception ex)
                {
                    Common.Api.Authorize(new VkNet.ApiAuthParams()
                    {
                        ApplicationId = Common.vkAppId,
                        Login = createInfo.Login,
                        Password = createInfo.Password,
                        Settings = VkNet.Enums.Filters.Settings.All
                    });
                }


                

                if (!IsActive)
                {
                    resp.Type = CommonDataTypes.Enums.ResponseTypes.FAILWithoutErrorMessage;
                    resp.Message = "Can't authorize with no reason";
                    return resp;
                }

                resp.Type = CommonDataTypes.Enums.ResponseTypes.OK;
                Token = Common.Api.Token;
           //     Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\DRAZDOV\\SOCIALNETWORKCLEANER", "LASTTOKEN", Token, Microsoft.Win32.RegistryValueKind.String);
                resp.Message = String.Format("Auth success, Token = {0}", Token);
            }
            catch (Exception ex)
            {
                resp.Type = CommonDataTypes.Enums.ResponseTypes.FAILWithErrorMessage;
                resp.Message = ex.Message;
            }

            return resp;            
            
        }
    }
}
