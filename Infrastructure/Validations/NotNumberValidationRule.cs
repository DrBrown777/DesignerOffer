using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Designer_Offer.Infrastructure.Validations
{
    internal class NotNumberValidationRule : ValidationRule
    {
        private static readonly string pattern = @"^\d+(?:\.\d{1,2})?$";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return !Regex.IsMatch(value.ToString(), pattern) ? new ValidationResult(false, "формат цены: 0.00") : ValidationResult.ValidResult;
        }
    }
}
