using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Lab02.Converters
{
    class TopLeftToCenterConverter: IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int v = (int)value;
            double p = (double)parameter;
            return System.Convert.ChangeType(v - p / 2, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double v = (double)value;
            double p = (double)parameter;
            return System.Convert.ChangeType(v + p / 2, targetType);
        }
    }
}
