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
using LinqToVisualTree;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace iot_core_qlock.Controls
{
    public sealed class Qlock : Control
    {
        public ViewModel.Clock.ClockData ClockData { get; private set; }
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
        public bool Shown
        {
            get { return (bool)GetValue(ShownProperty); }
            set { SetValue(ShownProperty, value); }
        }
        public static DependencyProperty ShownProperty = DependencyProperty.Register("Shown", typeof(bool), typeof(Qlock), new PropertyMetadata(true, new PropertyChangedCallback((A, B) => ((Qlock)A).OnShownChanged())));
        private void OnShownChanged()
        {
            if (Shown)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }

        ItemsControl _ItemsControl = null;
        Border _ContentBorder = null;
        public Qlock()
        {
            ClockData = new ViewModel.Clock.ClockData();
            DataContext = ClockData;
            this.DefaultStyleKey = typeof(Qlock);
            this.Loaded += Qlock_Loaded;
        }
        protected override void OnApplyTemplate()
        {
            _ItemsControl = GetTemplateChild("PART_ItemsControl") as ItemsControl;
            _ContentBorder = GetTemplateChild("PART_ContentBorder") as Border; 
        }
        private void Qlock_Loaded(object sender, RoutedEventArgs e)
        {
            ClockData.Run();
            OnShownChanged();
        }
        private IEnumerable<Storyboard> GetItemStoryboards(string storyBoardName)
        {
            return _ItemsControl.Descendants()
                .Where(A => A is FrameworkElement)
                .Where(A => (A as FrameworkElement).Name == "PART_Item")
                .Where(A=>(A as FrameworkElement).Resources.ContainsKey(storyBoardName))
                .Select(A => (A as FrameworkElement).Resources[storyBoardName] as Storyboard);
        }
        private Storyboard GetBorderStoryboard(string storyboardName)
        {
            if (_ContentBorder != null)
            {
                return _ContentBorder.Resources[storyboardName]as Storyboard;
            }

            return null;
        }
        private async void FadeIn()
        {
            foreach (var s in GetItemStoryboards("PART_FadeIn"))
            {
                s?.Begin();
                await Task.Delay(10);
            }

            GetBorderStoryboard("PART_FadeIn")?.Begin();
        }
        private async void FadeOut()
        {
            foreach (var s in GetItemStoryboards("PART_FadeOut"))
            {
                s?.Begin();
                await Task.Delay(10);
            }

            GetBorderStoryboard("PART_FadeOut")?.Begin();
        }
    }
}
