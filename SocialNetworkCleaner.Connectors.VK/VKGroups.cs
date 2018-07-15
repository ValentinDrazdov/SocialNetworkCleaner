using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors.VK
{
    public class VKGroups : IGroup
    {
        public GetAllSourcesNotificationEventHandler GetSourcesNotificationEvent;
        public List<CommonDataTypes.SocialElements.Group> GetGroups(CommonDataTypes.SocialElements.User user = null)
        {
            List<CommonDataTypes.SocialElements.Group> groups = null;
            if (user == null)
            {
                user = new CommonDataTypes.SocialElements.User() { Uid = Common.Api.UserId.Value };
            }
            
            var vkGroups = Common.Api.Groups.Get(new VkNet.Model.RequestParams.GroupsGetParams()
            {
                UserId = user.Uid,
                Count= 1000,
                Extended = true
                
            });

            groups = new List<CommonDataTypes.SocialElements.Group>(vkGroups.Count);
            ulong num = 1;
            ulong count = (ulong) vkGroups.Count;
            foreach (var vkGroup in vkGroups)
            {
                var group = new CommonDataTypes.SocialElements.Group()
                {
                    Uid = vkGroup.Id * -1,
                    Name = vkGroup.Name,
                    IsModerator = vkGroup.IsAdmin,
                    PictureURL = vkGroup.Photo100
                };
                if (GetSourcesNotificationEvent != null) GetSourcesNotificationEvent(count, num, group);
                groups.Add(group);
                num++;
            }

            return groups;
        }

        public CommonDataTypes.SocialElements.Group GetGroup(string groupID)
        {
            List<CommonDataTypes.SocialElements.Group> groups = null;

            var vkGroups = Common.Api.Groups.GetById(new List<String>() { groupID }, groupID, VkNet.Enums.Filters.GroupsFields.All);
            if (vkGroups == null) return null;
            if (vkGroups.Count == 0) return null;
            var vkGroup = vkGroups.First();

            CommonDataTypes.SocialElements.Group group = new CommonDataTypes.SocialElements.Group();
            group.Uid = vkGroup.Id * -1;
            group.Name = vkGroup.Name;
            group.IsModerator = vkGroup.IsAdmin;
            group.PictureURL = vkGroup.Photo100;
            return group;
        }
    }
}
