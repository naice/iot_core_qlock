using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace iot_core_qlock
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Browser : Page
    {
        class BrowserParam
        {
            public string URL { get; set; }
            public string Title { get; set; }
        }


        public Browser()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BrowserParam param = e.Parameter as BrowserParam;
            Title.Text = param.Title;

            if (param.URL != null)
            {
                try
                {
                    webView.Navigate(new Uri(param.URL, UriKind.Absolute));
                }
                catch 
                {
                    // ignore wrong uri
                }
            }
        }

        public static void Show(string url, string title, params object[] args)
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null && !frame.Navigate(typeof(Browser), new BrowserParam() {  URL = string.Format(url, args), Title = title}))
            {
                throw new Exception("Could not navigate to Browser!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null && frame.CanGoBack)
                frame.GoBack();
        }
    }
}
