using Designer_Offer.Data;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Designer_Offer.Infrastructure.Validations
{
    internal class UniqLoginValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                using (var context = new PrimeContext())
                {
                    if (context.UserData.AsNoTracking().Where(u => u.Login == value.ToString().Trim()).Any())
                    {
                        return new ValidationResult(false, "логин должен быть уникален");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return ValidationResult.ValidResult;
        }
    }
}
