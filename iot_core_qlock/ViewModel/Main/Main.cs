using iot_core_qlock.Common;
using iot_core_qlock.ViewModel.RSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iot_core_qlock.ViewModel.Main
{
    public class Main : ViewModelBase
    {
        public RSSReader RSSReader { get; set; }
        public ICommand ViewLoaded { get; set; }

        public Main()
        {
            ViewLoaded = new RelayCommand(OnViewLoaded);
        }

        private void OnViewLoaded(object obj)
        {
            RSSReader = new RSSReader(new Uri("http://rss.golem.de/rss.php?feed=RSS2.0"), new RSSCreatorGolem(), Global.UIDispatcher);
            RSSReader.StartAsync();
        }
    }
}
