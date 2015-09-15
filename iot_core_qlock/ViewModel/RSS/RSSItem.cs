using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iot_core_qlock.ViewModel.RSS
{
    public class RSSItem : ViewModelBase
    {
        public string Id { get; set; }

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
    }
}
