using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace iot_core_qlock.ViewModel.RSS
{
    public class RSSCreatorGolem : IRSSItemCreator
    {
        public string Name { get; } = "Golem";

        public Uri Url { get; } = new Uri("http://rss.golem.de/rss.php?feed=RSS2.0", UriKind.Absolute);

        public RSSItem CreateItem(RSSItem result, SyndicationItem item)
        {
            result.URIToSource = item.Id;

            if (!string.IsNullOrEmpty(result.ContentRaw))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(result.ContentRaw);
                foreach (HtmlNode link in doc.DocumentNode.Descendants("img"))
                {
                    result.Image = link.Attributes["src"].Value; break;
                }
                
                result.Content = HtmlEntity.DeEntitize(doc.DocumentNode.InnerText);
            }

            return result;
        }
    }
}
