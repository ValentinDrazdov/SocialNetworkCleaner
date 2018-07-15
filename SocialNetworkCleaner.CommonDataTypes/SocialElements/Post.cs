using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCleaner.CommonDataTypes.SocialElements
{
    public delegate void ImageLoadedDelegate();
    public delegate void SelectionDelegate(bool selection);
    public class Post
    {
        public event ImageLoadedDelegate ImageLoadedEventHandler;
        public event SelectionDelegate SelectionEventHandler;

        public string Guid { get; set; }
        public long Uid { get; set; }
        public int Id { get; set; }
        public long OwnerID { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }

        public bool? IsRepost { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public IPostSource Source { get; set; }
        public long? SourceID { get; set; }
        public long SourcePostUID { get; set; }

        private Uri _pictureURL;
        [System.Xml.Serialization.XmlIgnore]
        public Uri PictureURL
        {
            get
            {
                return _pictureURL;
            }
            set
            {
                _pictureURL = value;
                //LoadPicture();
            }
        }

        public String PictureURLString
        {
            get
            {
                if (_pictureURL == null) return null;
                return _pictureURL.ToString();
            }
            set
            {
                if (value == null) return;
                _pictureURL = new Uri(value);
            }
        }

        public void LoadPicture()
        {
            if (Picture == null)
            {
                String fileName = String.Format("imgs\\post{0}{1}.png", Uid, Id);
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
           
            if (Picture != null && ImageLoadedEventHandler != null)
            {
                ImageLoadedEventHandler();
            }
        }
        public void Selection(bool selection)
        {
            if (SelectionEventHandler != null)
            {
                SelectionEventHandler(selection);
            }
        }

        public byte[] Picture { get; set; }
    }
}
