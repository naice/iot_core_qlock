using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace iot_core_qlock.ViewModel.RSS
{
    public class RSSCreatorT3N : IRSSItemCreator
    {
        const int MAX_NEWS_LEN = 300;

        public string Name { get; } = "t3n";

        public Uri Url { get; } = new Uri("http://t3n.de/news/feed/", UriKind.Absolute);

        public RSSItem CreateItem(RSSItem result, SyndicationItem item)
        {
            result.URIToSource = item.Id;

            if (!string.IsNullOrEmpty(result.ContentRaw))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(result.ContentRaw);
                
                foreach (HtmlNode link in doc.DocumentNode.Descendants("img"))
                {
                    int width = Convert.ToInt32(link.Attributes["width"]?.Value ?? "9999");
                    int height = Convert.ToInt32(link.Attributes["height"]?.Value ?? "9999");

                    if (width > 10 && height > 10)
                    {
                        result.Image = link.Attributes["src"].Value; break;
                    }
                }

                result.Content = "";

                var p = doc.DocumentNode.Descendants("p").FirstOrDefault();
                if (p != null)
                {
                    result.Content = HtmlEntity.DeEntitize(p.InnerText).Replace("\n", "").Replace("\r\n", "").Trim();
                }

                if (string.IsNullOrWhiteSpace(result.Content))
                {
                    result.Content = HtmlEntity.DeEntitize(doc.DocumentNode.InnerText).Replace("\n", "").Replace("\r\n", "").Trim();
                }

                if (!string.IsNullOrEmpty(result.Content))
                {
                    result.Content = result.Content.Length > MAX_NEWS_LEN ? result.Content.Substring(0, MAX_NEWS_LEN) : result.Content;
                }
            }

            return result;
        }
    }
}
