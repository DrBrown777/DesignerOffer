using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Designer_Offer.Infrastructure.Validations
{
    internal class NotNumberValidationRule : ValidationRule
    {
        //private static readonly string pattern = @"^(?<!\d)(?<!\d[.,])\d{1,20}(?:[.,]\d{2})*(?![.,]?\d)$";
        private static readonly string pattern = @"^[+-]?[0-9]+\.[0-9]+$";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return !Regex.IsMatch(value.ToString(), pattern) ? new ValidationResult(false, "формат 0.00") : ValidationResult.ValidResult;
        }
    }
}
