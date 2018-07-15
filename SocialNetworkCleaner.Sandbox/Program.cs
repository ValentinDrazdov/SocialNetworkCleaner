using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            // обработчик получения кода
            Func<string> code = () =>
            {
                Console.Write("Please enter code: ");
                string value = Console.ReadLine().Trim();
                return value;
            };

            SocialNetworkCleaner.Connectors.VK.VKSession session = new Connectors.VK.VKSession();
            try
            {

                session.Create(new CommonDataTypes.CreateSessionData()
                {
                    Login = "",
                    Password = "",
                    TwoFactorAuthorization = code
                });
            }
            catch (Exception ex)
            {

                Console.Out.WriteLine(ex.Message);
            }

            Connectors.IProcessor processor = new Connectors.VK.VKProcessor();
            processor.GetPostsNotificationEvent += Processor_GetPostsNotificationEvent;
            var user = (new Connectors.VK.VKUser()).GetUserInfo();
            var posts = processor.GetAllPosts(user);

            var groups = processor.GetAllUsersGroups(user);
            posts = processor.AddInformationAboutSourceToPosts(posts, user, false);

            processor.SaveData(posts);
        }

        private static void Processor_GetPostsNotificationEvent(ulong count, ulong index, CommonDataTypes.SocialElements.Post post)
        {
            Console.Write(String.Format("{0}/{1} - {2}\n\r", index, count, post.Uid));
        }
    }
}
