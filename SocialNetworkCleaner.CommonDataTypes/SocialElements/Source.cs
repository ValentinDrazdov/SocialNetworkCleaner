using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.CommonDataTypes.SocialElements
{
    public class Source 
    {
        public string Guid { get; set; }
        public long Uid { get; set; }
        public int Id { get; set; }
        public string DisplayName { get; set; }
        private Uri _pictureURL;

        [System.Xml.Serialization.XmlIgnore]
        public Uri PictureURL {
            get
            {
                return _pictureURL;
            }
            set
            {
                _pictureURL = value;
                LoadPicture();
            }
        }

        public void LoadPicture()
        {
            String fileName = String.Format("imgs\\source{0}{1}.png", Uid, Id);
            try
            {
                if (System.IO.Directory.Exists("imgs"))
                {
                    if (System.IO.File.Exists(fileName))
                    {
                        Picture = System.IO.File.ReadAllBytes(fileName);
                    }
                }
                else
                {
                    System.IO.Directory.CreateDirectory("imgs");
                }
            }
            catch (Exception ex)
            {
                Picture = null;
            }
            if (Picture == null && PictureURL != null)
            {
                try
                {
                    Picture = (new System.Net.WebClient()).DownloadData(PictureURL);
                    System.IO.File.WriteAllBytes(fileName, Picture);
                }
                catch (Exception ex)
                {
                    Picture = null;
                }
            }

        }
        
        public byte[] Picture { get; set; }
    }
}
