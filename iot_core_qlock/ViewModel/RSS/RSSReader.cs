using HtmlAgilityPack;
using iot_core_qlock.Common;
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
        /// <summary>
        /// Model.
        /// </summary>
        public SortedObservableCollection<RSSItem> ItemsSource { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(10);

        private SyndicationClient _SyndicationClient = new SyndicationClient();
        private IRSSItemCreator[] _Creators;
        private Windows.UI.Core.CoreDispatcher _Dispatcher = null;

        public RSSReader(IRSSItemCreator creator, Windows.UI.Core.CoreDispatcher dispatcher)
            : this(new IRSSItemCreator[] { creator }, dispatcher)
        {
        }
        public RSSReader(IEnumerable<IRSSItemCreator> creators, Windows.UI.Core.CoreDispatcher dispatcher)
        {
            _Dispatcher = dispatcher;
            _Creators = creators.ToArray();
            ItemsSource = new SortedObservableCollection<RSSItem>(T=>T.Created.Ticks, SortedObservableCollectionSortDirection.DESC);
        }

        public async void StartAsync()
        {
            foreach (var creator in _Creators)
            {
                var feed = await _SyndicationClient.RetrieveFeedAsync(creator.Url);

                if (feed != null && feed.Items != null && feed.Items.Count > 0)
                {
                    foreach (var item in feed.Items)
                    {
                        Windows.UI.Core.DispatchedHandler handler = () =>
                        {
                            bool isNew = false;
                            RSSItem rssItem = ItemsSource.FirstOrDefault(A => A.Id == item.Id);
                            if (rssItem == null)
                            {
                                isNew = true;
                                rssItem = new RSSItem()
                                {
                                    Id = item.Id,
                                    Source = creator.Name,
                                    Title = item.Title?.Text ?? string.Empty,
                                };
                            }
                            string rawContent = item.Content?.Text ?? item.Summary?.Text ?? string.Empty;
                            rssItem.ContentRaw = rawContent ?? rssItem.ContentRaw;
                            rssItem.Created = item.PublishedDate.DateTime;

                            rssItem = creator.CreateItem(rssItem, item);
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
}
