using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace iot_core_qlock.ViewModel.RSS
{
    public interface IRSSItemCreator
    {
        RSSItem CreateItem(RSSItem result, SyndicationItem item, string rawContent);
    }
}
