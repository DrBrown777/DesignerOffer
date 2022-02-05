using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Designer_Offer.Infrastructure.Validations
{
    internal class NotValidPhoneValidationRule : ValidationRule
    {
        //private static readonly string pattern = @"^((\+?3)?8)?0\d{9}$";
        private static readonly string pattern = @"^((\+?3)?8)?((0\(\d{2}\)?)|(\(0\d{2}\))|(0\d{2}))\d{7}$";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return !Regex.IsMatch(value.ToString(), pattern) ? new ValidationResult(false, "формат номера +38(067)5552233") : ValidationResult.ValidResult;
        }
    }
}
