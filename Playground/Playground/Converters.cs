using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    using System;

    public class Dec2Float : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameters, System.Globalization.CultureInfo culture)
        {
            return (double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameters, System.Globalization.CultureInfo culture)
        {
            decimal val;
            if (value == null) return 0;
            decimal.TryParse(value.ToString(), out val);
            return val;
        }
    }
}
