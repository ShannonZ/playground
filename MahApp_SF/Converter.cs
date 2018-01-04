using System;

namespace MahApp_SF
{
        public class Error2Status : System.Windows.Data.IValueConverter
        {

            public object Convert(object value, Type targetType, object parameters, System.Globalization.CultureInfo culture)
            {
                if ((int)value == 0)
                    return true;
                else
                    return false;
            }

            public object ConvertBack(object value, Type targetType, object parameters, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    public class Dec2Float : System.Windows.Data.IValueConverter
    {

        public object Convert(object value, Type targetType, object parameters, System.Globalization.CultureInfo culture)
        {
            return (float)value;
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
