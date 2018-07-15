using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors.VK
{
    public class VKPosts : IPosts
    {
        public event GetAllPostsNotificationEventHandler GetPostNotificationEvent;

        public void DeletePost(long uid)
        {
            VK.Common.Api.Wall.Delete(Common.Api.UserId.Value, uid);
        }

        public List<CommonDataTypes.SocialElements.Post> FromGroup(CommonDataTypes.SocialElements.Group group, ulong count, ulong offset)
        {
            throw new NotImplementedException();
        }

        public CommonDataTypes.SocialElements.Post FromGroupByID(CommonDataTypes.SocialElements.Group group, CommonDataTypes.SocialElements.Post IDInform)
        {
            throw new NotImplementedException();
        }


        public List<CommonDataTypes.SocialElements.Post> FromUser(CommonDataTypes.SocialElements.User user, ulong count, ulong offset)
        {
            List<CommonDataTypes.SocialElements.Post> posts = null;
            if (user == null)
            {
                user = new CommonDataTypes.SocialElements.User() { Uid = Common.Api.UserId.Value };
            }

            var vkPosts = Common.Api.Wall.Get(new VkNet.Model.RequestParams.WallGetParams()
            {
                Count = count,
                Offset = offset,
                OwnerId = user.Uid
            });

            posts = new List<CommonDataTypes.SocialElements.Post>(vkPosts.WallPosts.Count);
            ulong index = 0;
            foreach (var vkPost in vkPosts.WallPosts)
            {
                index++;

                var post = new CommonDataTypes.SocialElements.Post()
                {
                    Uid = vkPost.Id.Value,
                    Text = vkPost.Text,
                    Date = vkPost.Date.Value.ToString("dd.MM.yyyy hh:mm:ss"),
                    IsRepost = (vkPost.CopyHistory != null && vkPost.CopyHistory.Count > 0) ? true : false,
                    OwnerID = vkPost.OwnerId.Value
                    
                };

                try
                {
                    LoadPictureIfNull(post, vkPost);
                    if (post.IsRepost.Value)
                    {
                        post.SourceID = vkPost.CopyHistory.First().FromId.Value;
                        post.SourcePostUID = vkPost.CopyHistory.First().Id.Value;
                        foreach (var copyHistoryPart in vkPost.CopyHistory)
                        {
                            LoadPictureIfNull(post, copyHistoryPart);
                            post.Text = String.Format("{0}\n\n+++\n\n{1}", post.Text, copyHistoryPart.Text);
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                posts.Add(post );
                if (GetPostNotificationEvent != null) GetPostNotificationEvent(offset + count, offset + index, post);


            }

            return posts;
        }

        private void LoadPictureIfNull (CommonDataTypes.SocialElements.Post post, VkNet.Model.Post vkPost)
        {
            if (post.Picture != null) return;
            if (vkPost == null) return;

            if (vkPost.Attachments == null) return;
            if (vkPost.Attachments.Count == 0) return;

            foreach (var attachment in vkPost.Attachments)
            {
                if (attachment.Instance == null) continue;
                if (attachment.Instance is VkNet.Model.Attachments.Photo)
                {
                    var photo = ((VkNet.Model.Attachments.Photo)vkPost.Attachments[0].Instance);
                    if (photo.Photo604 != null)
                    {
                        post.PictureURL = photo.Photo604;
                    }
                    else if (photo.Photo130 != null)
                    {
                        post.PictureURL = photo.Photo130;
                    }
                    else if (photo.Photo75 != null)
                    {
                        post.PictureURL = photo.Photo75;
                    }

                    if (post.Picture != null) return;
                }
            }
        }

        public CommonDataTypes.SocialElements.Post FromUserByID(CommonDataTypes.SocialElements.User user, CommonDataTypes.SocialElements.Post IDInform)
        {
            throw new NotImplementedException();
        }
    }
}
