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

        private RSSReader _RSSReader;
        public RSSReader RSSReader
        {
            get { return _RSSReader; }
            set
            {
                if (value != _RSSReader)
                {
                    _RSSReader = value;
                    RaisePropertyChanged("RSSReader");
                }
            }
        }

        private bool _IsQlockShown = true;
        public bool IsQlockShown
        {
            get { return _IsQlockShown; }
            set
            {
                if (value != _IsQlockShown)
                {
                    _IsQlockShown = value;
                    RaisePropertyChanged("IsQlockShown");
                }
            }
        }
        public ICommand ViewLoaded { get; set; }
        public ICommand QlockTapped { get; set; }
        public ICommand RSSItemClicked { get; set; }
        public ICommand RSSItemHolding { get; set; }

        public Main()
        {
            ViewLoaded = new RelayCommand<object>(OnViewLoaded);
            QlockTapped = new RelayCommand<object>(OnQlockTapped);
            RSSItemClicked = new RelayCommand<Windows.UI.Xaml.Controls.ItemClickEventArgs>(OnRSSItemClick);
            RSSItemHolding = new RelayCommand<object>(OnItemHolding);
        }

        private void OnItemHolding(object obj)
        {

        }

        private void OnQlockTapped(object obj)
        {
            IsQlockShown = false;
        }

        private void OnViewLoaded(object obj)
        {
            IsQlockShown = true;
            RSSReader = new RSSReader(
                new IRSSItemCreator[] {
                    new RSSCreatorGolem(),
                    new RSSCreatorT3N(),
                },
                Global.UIDispatcher);
            RSSReader.StartAsync();
        }

        private void OnRSSItemClick(Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            RSSItem item = e.ClickedItem as RSSItem;
            Browser.Show(item.URIToSource, item.Title);
        }
    }
}
