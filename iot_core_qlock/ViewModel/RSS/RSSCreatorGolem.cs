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
        public RSSItem CreateItem(RSSItem result, SyndicationItem item, string rawContent)
        {
            result.URIToSource = item.Id;

            if (!string.IsNullOrEmpty(rawContent))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(rawContent);
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
