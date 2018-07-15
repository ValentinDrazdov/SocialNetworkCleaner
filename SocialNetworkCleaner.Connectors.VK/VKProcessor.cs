using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Connectors.VK
{
    public class VKProcessor : IProcessor
    {

        private List<CommonDataTypes.SocialElements.Group> _knownGroups;
        private List<CommonDataTypes.SocialElements.User> _knownUsers;


        public event GetAllPostsNotificationEventHandler GetPostsNotificationEvent;
        public event GetAllSourcesNotificationEventHandler GetSourcesNotificationEvent;
        public event AddInformationAboutSourceToPostsEventHandler AddInformationAboutSourceToPostsEvent;

        public List<CommonDataTypes.SocialElements.Group> KnownGroups
        {
            get
            {
                if (_knownGroups == null) _knownGroups = new List<CommonDataTypes.SocialElements.Group>(1000);
                return _knownGroups;
            }
        }
        public List<CommonDataTypes.SocialElements.User> KnownUsers
        {
            get
            {
                if (_knownUsers == null) _knownUsers = new List<CommonDataTypes.SocialElements.User>(1000);
                return _knownUsers;
            }
        }



        public List<CommonDataTypes.SocialElements.Post> AddInformationAboutSourceToPosts(List<CommonDataTypes.SocialElements.Post> posts, CommonDataTypes.SocialElements.User currentUser, bool loadPictures)
        {
            IGroup groups = new VKGroups();
            IUser users = new VKUser();
            CommonDataTypes.SocialElements.Group _group = null;
            CommonDataTypes.SocialElements.User _user = null;

            var searchCurrentUser = KnownUsers.Where(x => x.Uid == currentUser.Uid).ToList();
            if (searchCurrentUser.Count == 0)
            {
                KnownUsers.Add(currentUser);
            }

            ulong count = (ulong) posts.Count, index = 0;
            foreach (var post in posts)
            {
                index++;
                try
                {
                    if (post.IsRepost.Value)
                    {
                        if (post.SourceID < 0)
                        {
                            var searchGroup = KnownGroups.Where(x => x.Uid == post.SourceID || x.Uid == post.SourceID * -1).ToList();
                            if (searchGroup.Count > 0)
                            {
                                _group = searchGroup.First();
                            }
                            else
                            {
                                _group = groups.GetGroup(((int)post.SourceID * -1).ToString());
                                KnownGroups.Add(_group);
                                System.Threading.Thread.Sleep(200);
                            }

                            post.Source = _group;
                        }
                        else
                        {
                            var searchUser = KnownUsers.Where(x => x.Uid == post.SourceID).ToList();
                            if (searchUser.Count > 0)
                            {
                                _user = searchUser.First();
                            }
                            else
                            {
                                _user = new CommonDataTypes.SocialElements.User() { Uid = (int)post.SourceID };
                                _user = users.GetUserInfo(_user);
                                KnownUsers.Add(_user);
                                System.Threading.Thread.Sleep(200);
                            }
                            post.Source = _user;
                        }
                    }
                    else
                    {
                        post.SourceID = currentUser.Uid;
                        post.Source = currentUser;
                    }

                    if (loadPictures)
                    {
                        post.LoadPicture();
                    }
                    
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(1000);
                }
                if (AddInformationAboutSourceToPostsEvent != null) AddInformationAboutSourceToPostsEvent(count, index, post);
               // if (GetGroupsNotificationEvent != null) GetGroupsNotificationEvent(count, index, post);
            }

            return posts;
        }

        public List<CommonDataTypes.SocialElements.Group> GetAllUsersGroups(CommonDataTypes.SocialElements.User user)
        {
            var vkGroups = new Connectors.VK.VKGroups();
            vkGroups.GetSourcesNotificationEvent += Sources_GetSourceNotificationEvent;
            List<CommonDataTypes.SocialElements.Group> groups = vkGroups.GetGroups(user);
            var UnknownGroups = groups.Where(x => KnownGroups.Where(y => y.Uid == x.Uid).Count() == 0);
            if (UnknownGroups.Count() > 0)
            {
                KnownGroups.AddRange(UnknownGroups.ToList());
            }            
            return groups;
        }
        public List<CommonDataTypes.SocialElements.User> GetAllUsersFriends(CommonDataTypes.SocialElements.User user)
        {
            var vkUser = new Connectors.VK.VKUser();
            vkUser.GetSourcesNotificationEvent += Sources_GetSourceNotificationEvent;
            List<CommonDataTypes.SocialElements.User> friends = vkUser.GetUsers(user);
            var UnknownUsers = friends.Where(x => KnownUsers.Where(y => y.Uid == x.Uid).Count() == 0);
            if (UnknownUsers.Count() > 0)
            {
                KnownUsers.AddRange(UnknownUsers.ToList());
            }
            return friends;
        }

        private void Sources_GetSourceNotificationEvent(ulong count, ulong index, CommonDataTypes.SocialElements.IPostSource source)
        {
            if (GetSourcesNotificationEvent != null) GetSourcesNotificationEvent(count, index, source);
        }
        public List<CommonDataTypes.SocialElements.Post> GetAllPosts(CommonDataTypes.SocialElements.User user, long limit = 0)
        {
            if (limit <= 0)
            {
                limit = long.MaxValue;
            }
            IPosts posts = new VKPosts();
            if (user == null)
            {
                user = new CommonDataTypes.SocialElements.User() { Uid = Common.Api.UserId.Value };
            }

            posts.GetPostNotificationEvent += Posts_GetPostNotificationEvent;
            List<CommonDataTypes.SocialElements.Post> allPosts = new List<CommonDataTypes.SocialElements.Post>(100000);
            List<CommonDataTypes.SocialElements.Post> currentPosts = null;
            ulong offset = 0;
            try
            {

                do
                {
                    currentPosts = posts.FromUser(user, 100, offset);
                    offset += 100;
                    allPosts.AddRange(currentPosts);
                    System.Threading.Thread.Sleep(200);
                } while (currentPosts != null && currentPosts.Count > 0 && allPosts.Count < limit);
            }
            catch (Exception ex)
            {

            }
            return allPosts;
        }

        private void Posts_GetPostNotificationEvent(ulong count, ulong index, CommonDataTypes.SocialElements.Post post)
        {
            if (GetPostsNotificationEvent != null) GetPostsNotificationEvent(count, index, post);
        }

        public void DeletePost(long postUID)
        {

            IPosts posts = new VKPosts();
            posts.DeletePost(postUID);
        }

        public void SaveData(List<CommonDataTypes.SocialElements.Post> posts)
        {
            
            System.Xml.Serialization.XmlSerializer xsSubmit = new System.Xml.Serialization.XmlSerializer(typeof(List<CommonDataTypes.SocialElements.Post>));

            var xml = "";

            using (var sww = new System.IO.StreamWriter("posts.xml"))
            {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, posts);
                    xml = sww.ToString(); // Your XML
                }
            }

            xsSubmit = new System.Xml.Serialization.XmlSerializer(typeof(List<CommonDataTypes.SocialElements.Group>));
            using (var sww = new System.IO.StreamWriter("groups.xml"))
            {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, KnownGroups);
                    xml = sww.ToString(); // Your XML
                }
            }

            xsSubmit = new System.Xml.Serialization.XmlSerializer(typeof(List<CommonDataTypes.SocialElements.User>));
            using (var sww = new System.IO.StreamWriter("users.xml"))
            {
                using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, KnownUsers);
                    xml = sww.ToString(); // Your XML
                }
            }
        }

        public List<CommonDataTypes.SocialElements.Post> LoadData()
        {
            ulong num = 0;
            ulong count = 1;
            List<CommonDataTypes.SocialElements.Post> posts;
            System.Xml.Serialization.XmlSerializer xsSubmit;

            if (GetSourcesNotificationEvent != null) GetSourcesNotificationEvent(count, num, new SocialNetworkCleaner.CommonDataTypes.SocialElements.User() { DisplayName = "users.xml" });
            xsSubmit = new System.Xml.Serialization.XmlSerializer(typeof(List<CommonDataTypes.SocialElements.User>));
            using (var rdr = new System.IO.StreamReader("users.xml"))
            {
                using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(rdr))
                {
                    KnownUsers.AddRange((List<CommonDataTypes.SocialElements.User>)xsSubmit.Deserialize(reader));

                }
            }

            if (GetSourcesNotificationEvent != null) GetSourcesNotificationEvent(count, num, new SocialNetworkCleaner.CommonDataTypes.SocialElements.Group() { DisplayName = "groups.xml" });
            xsSubmit = new System.Xml.Serialization.XmlSerializer(typeof(List<CommonDataTypes.SocialElements.Group>));
            using (var rdr = new System.IO.StreamReader("groups.xml"))
            {
                using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(rdr))
                {
                    KnownGroups.AddRange((List<CommonDataTypes.SocialElements.Group>)xsSubmit.Deserialize(reader));

                }
            }


            if (GetPostsNotificationEvent != null) GetPostsNotificationEvent(count, num, new CommonDataTypes.SocialElements.Post() { Uid = 0 });
            xsSubmit = new System.Xml.Serialization.XmlSerializer(typeof(List<CommonDataTypes.SocialElements.Post>));

            using (var rdr = new System.IO.StreamReader("posts.xml"))
            {
                using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(rdr))
                {
                    posts = (List <CommonDataTypes.SocialElements.Post>) xsSubmit.Deserialize(reader);
                    
                }
            }

            count = (ulong) posts.Count();
            foreach (var post in posts)
            {
                num++;
                if (GetPostsNotificationEvent != null) GetPostsNotificationEvent(count, num, post);
                var searchGroup = KnownGroups.Where(x => x.Uid == post.SourceID).ToList();
                if (searchGroup.Count > 0)
                {
                    post.Source = searchGroup.First();
                }
                else
                {
                    var searchUser = KnownUsers.Where(x => x.Uid == post.SourceID).ToList();
                    if (searchUser.Count > 0)
                    {
                        post.Source = searchUser.First();
                    }
                }
            }

            return posts;
        }
    }
}
