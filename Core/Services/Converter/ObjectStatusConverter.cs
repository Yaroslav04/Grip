using Android.Provider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grip.Core.Services.Converter
{
    internal class ObjectStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int i = System.Convert.ToInt32(value);

            if (i == 0)
            {
                return "на виконанні";
            }

            if (i == 1)
            {
                return "виконано";
            }

            if (i == 2)
            {
                return "не виконано";
            }

            return "помилка";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}
