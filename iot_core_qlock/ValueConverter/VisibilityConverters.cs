using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace iot_core_qlock.ValueConverter
{
    public class GenericVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string parm = (string)parameter;
            bool show = false;

            if (value is string || value is String)
            {
                if (parameter is string && parm.Contains("nullorempty"))
                {
                    show = !string.IsNullOrEmpty(value as string);
                }
                else if (parameter is string && parm.Contains("nullorwhitespace"))
                {
                    show = !string.IsNullOrWhiteSpace(value as string);
                }
                else if (parameter is string && parm.Contains("url"))
                {
                    if (value != null)
                        show = Uri.IsWellFormedUriString(value as string, UriKind.Absolute);
                }
                else
                {
                    show = value != null;
                }


            }
            else if (value is bool || value is Boolean)
            {
                show = (bool)value;
            }
            else if (value is int)
            {
                show = (int)value > 0;
            }
            else if (value is long)
            {
                show = (long)value > 0;
            }
            else if (value is double)
            {
                show = (double)value > 0;
            }
            else
            {
                show = value != null;
            }
            
            if (parameter is string && parm.Contains("invert"))
                show = !show;

#if DEBUG
            if (parameter is string && parm.Contains("echo"))
                Debug.WriteLine("GenericVisibilityConverter: SHOW:{0} VALUE:{1}", show, value);

#endif

            return show ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
