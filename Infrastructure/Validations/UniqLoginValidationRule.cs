using Designer_Offer.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Designer_Offer.Infrastructure.Validations
{
    internal class UniqLoginValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            using(var context = new PrimeContext())
            {
                if (context.UserData.AsNoTracking().Where(u => u.Login == value.ToString().Trim()).Any())
                {
                    return new ValidationResult(false, "логин должен быть уникален");
                }
                    
                return ValidationResult.ValidResult;
            }
        }
    }
}
