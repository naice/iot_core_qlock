using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace iot_core_qlock
{
    static class Global
    {
        public static CoreDispatcher UIDispatcher => CoreWindow.GetForCurrentThread()?.Dispatcher;
    }
}
