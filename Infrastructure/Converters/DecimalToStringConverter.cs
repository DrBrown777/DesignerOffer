using Designer_Offer.Infrastructure.Converters.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer_Offer.Infrastructure.Converters
{
    internal class DecimalToStringConverter : Converter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            return decimal.Parse(v.ToString());
        }

        public override object ConvertBack(object va, Type t, object p, CultureInfo c)
        {
            return va.ToString();
        }
    }
}
