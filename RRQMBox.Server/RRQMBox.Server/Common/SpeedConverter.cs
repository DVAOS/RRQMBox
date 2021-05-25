using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RRQMBox.Server.Common
{
    public class SpeedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (long)value / (1024 * 1024.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double num = double.Parse(value.ToString());
                return (long)(num * (1024 * 1024));
            }
            catch (Exception)
            {
                return 1024 * 1024;
            }
        }
    }
}
