using Designer_Offer.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Designer_Offer.Infrastructure.Validations
{
    internal class UniqPhoneValidationRule : ValidationRule
    {
        private static readonly string pattern = @"^((\+?3)?8)?0\d{9}$";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            using(var context = new PrimeContext())
            {
                if(!Regex.IsMatch(value.ToString(), pattern))
                {
                    return new ValidationResult(false, "формат номера +380675552233");
                }
                else if (context.Employee.AsNoTracking().Where(e => e.Phone == value.ToString().Trim()).Any())
                {
                    return new ValidationResult(false, "телефон должен быть уникален");
                }

                return ValidationResult.ValidResult;
            }
        }
    }
}
