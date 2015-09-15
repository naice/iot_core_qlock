using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace iot_core_qlock.ViewModel.RSS
{
    public class RSSReader : ViewModelBase
    {
        public ObservableCollection<RSSItem> ItemsSource { get; set; }

        public Uri FeedUri { get; private set; }

        private SyndicationClient _SyndicationClient = new SyndicationClient();
        private IRSSItemCreator _Creator;
        private Windows.UI.Core.CoreDispatcher _Dispatcher = null;

        public RSSReader(Uri feedUri, IRSSItemCreator creator, Windows.UI.Core.CoreDispatcher dispatcher)
        {
            _Dispatcher = dispatcher;
            _Creator = creator;
            ItemsSource = new ObservableCollection<RSSItem>();
            FeedUri = feedUri;
        }

        public async void StartAsync()
        {
            var feed = await _SyndicationClient.RetrieveFeedAsync(FeedUri);

            if (feed != null && feed.Items != null && feed.Items.Count > 0)
            {
                foreach (var item in feed.Items)
                {
                    Windows.UI.Core.DispatchedHandler handler = () => {
                        bool isNew = false;
                        RSSItem rssItem = ItemsSource.FirstOrDefault(A => A.Id == item.Id);
                        if (rssItem == null)
                        {
                            isNew = true;
                            rssItem = new RSSItem()
                            {
                                Id = item.Id,
                                Title = item.Title != null ? item.Title.Text : "",
                            };
                        }
                        string rawContent = "";
                        if (item.Content != null) rawContent = item.Content.Text;
                        else if (item.Summary != null) rawContent = item.Summary.Text;
                        rssItem.Created = item.PublishedDate.DateTime;

                        rssItem = _Creator.CreateItem(rssItem, item, rawContent);

                        if (isNew)
                            ItemsSource.Add(rssItem);
                    };

                    if (!_Dispatcher.HasThreadAccess)
                        await _Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, handler);
                    else
                        handler();
                }
            }
        }
    }
}
