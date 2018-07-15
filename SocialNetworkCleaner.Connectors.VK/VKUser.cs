using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors.VK
{
    public class VKUser : IUser
    {
        public GetAllSourcesNotificationEventHandler GetSourcesNotificationEvent;
        public CommonDataTypes.SocialElements.User GetUserInfo(CommonDataTypes.SocialElements.User idInfo = null)
        {
            if (idInfo == null)
            {
                idInfo = new CommonDataTypes.SocialElements.User() { Uid = VK.Common.Api.UserId.Value };
            }
            var userInfos = VK.Common.Api.Users.Get(new List<long>() { idInfo.Uid}, VkNet.Enums.Filters.ProfileFields.All);

            if (userInfos == null) return null;
            if (userInfos.Count == 0) return null;

            var userInfo = userInfos.First();

            idInfo.Name = userInfo.FirstName;
            idInfo.SurName = userInfo.LastName;
            idInfo.NickName = userInfo.Nickname;
            idInfo.PictureURL = userInfo.Photo100;
            idInfo.HomePhone = userInfo.HomePhone;
            idInfo.MobilePhone = userInfo.MobilePhone;
            idInfo.BirthDate = userInfo.BirthDate;
            if (userInfo.Occupation != null) idInfo.Occupation = userInfo.Occupation.Name;
            return idInfo;
        }

        public List<CommonDataTypes.SocialElements.User> GetUsers(CommonDataTypes.SocialElements.User user = null)
        {
            List<CommonDataTypes.SocialElements.User> users = null;
            if (user == null)
            {
                user = new CommonDataTypes.SocialElements.User() { Uid = Common.Api.UserId.Value };
            }

            var vkUsers = Common.Api.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams()
            {
                UserId = user.Id,
                Fields = VkNet.Enums.Filters.ProfileFields.FirstName |
                VkNet.Enums.Filters.ProfileFields.LastName |
                VkNet.Enums.Filters.ProfileFields.Nickname |
                VkNet.Enums.Filters.ProfileFields.Photo100 |
                VkNet.Enums.Filters.ProfileFields.Contacts |
                VkNet.Enums.Filters.ProfileFields.BirthDate


            });

            users = new List<CommonDataTypes.SocialElements.User>(vkUsers.Count);
            ulong num = 1;
            ulong count = (ulong)vkUsers.Count;
            foreach (var vkUser in vkUsers)
            {
                CommonDataTypes.SocialElements.User newUser = new CommonDataTypes.SocialElements.User();
                newUser.Uid = vkUser.Id;
                newUser.Name = vkUser.FirstName;
                newUser.SurName = vkUser.LastName;
                newUser.NickName = vkUser.Nickname;
                newUser.PictureURL = vkUser.Photo100;
                newUser.HomePhone = vkUser.HomePhone;
                newUser.MobilePhone = vkUser.MobilePhone;
                newUser.BirthDate = vkUser.BirthDate;

                if (GetSourcesNotificationEvent != null) GetSourcesNotificationEvent(count, num, newUser);

                users.Add(newUser);
                num++;
            }

            return users;
        }
    }
}
