using iot_core_qlock.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iot_core_qlock.ViewModel.RSS
{
    public class RSSItem : ViewModelBase
    {
        public string Id { get; set; }


        private string _Source;
        public string Source
        {
            get { return _Source; }
            set
            {
                if (value != _Source)
                {
                    _Source = value;
                    RaisePropertyChanged("Source");
                }
            }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (value != _Title)
                {
                    _Title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }
        private string _Content;
        public string Content
        {
            get { return _Content; }
            set
            {
                if (value != _Content)
                {
                    _Content = value;
                    RaisePropertyChanged("Content");
                }
            }
        }


        private string _ContentRaw;
        public string ContentRaw
        {
            get { return _ContentRaw; }
            set
            {
                if (value != _ContentRaw)
                {
                    _ContentRaw = value;
                    RaisePropertyChanged("ContentRaw");
                }
            }
        }

        private string _Image;
        public string Image
        {
            get { return _Image; }
            set
            {
                if (value != _Image)
                {
                    _Image = value;
                    RaisePropertyChanged("Image");
                }
            }
        }

        private string _URIToSource;
        public string URIToSource
        {
            get { return _URIToSource; }
            set
            {
                if (value != _URIToSource)
                {
                    _URIToSource = value;
                    RaisePropertyChanged("URIToSource");
                }
            }
        }

        private DateTime _Created;
        public DateTime Created
        {
            get { return _Created; }
            set
            {
                if (value != _Created)
                {
                    _Created = value;
                    RaisePropertyChanged("Created");
                }
            }
        }
        
        private int _ImageWidth = 120;
        public int ImageWidth
        {
            get { return _ImageWidth; }
            set
            {
                if (value != _ImageWidth)
                {
                    _ImageWidth = value;
                    RaisePropertyChanged("ImageWidth");
                }
            }
        }

        public ICommand ImageLoadFailed { get; private set; }

        public RSSItem()
        {
            ImageLoadFailed = new RelayCommand<object>(OnImageFailed);
        }

        private void OnImageFailed()
        {
            Image = null;
        }
    }
}
