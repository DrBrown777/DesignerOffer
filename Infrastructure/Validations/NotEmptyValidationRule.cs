using System.Globalization;
using System.Windows.Controls;

namespace Designer_Offer.Infrastructure.Validations
{
    internal class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString()) ? new ValidationResult(false, "обязательное поле") : ValidationResult.ValidResult;
        }
    }
}
