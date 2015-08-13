using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace iot_core_qlock.Controls
{
    public sealed class Qlock : Control
    {
        public ViewModel.Clock.ClockData ClockData { get; set; }

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        public static DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(Qlock), new PropertyMetadata(64, new PropertyChangedCallback((A, B) => ((Qlock)A).OnItemWidthChanged())));
        private void OnItemWidthChanged()
        {
            Width = ItemWidth * 11;
        }

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        public static DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(Qlock), new PropertyMetadata(64, new PropertyChangedCallback((A, B) => ((Qlock)A).OnItemHeightChanged())));
        private void OnItemHeightChanged()
        {
            Height = ItemHeight * 10;
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(Qlock), new PropertyMetadata(null));




        public Qlock()
        {
            ClockData = new ViewModel.Clock.ClockData();
            DataContext = ClockData;
            this.DefaultStyleKey = typeof(Qlock);
            this.Loaded += Qlock_Loaded;
        }

        private void Qlock_Loaded(object sender, RoutedEventArgs e)
        {
            ClockData.Run();
        }
    }
}
