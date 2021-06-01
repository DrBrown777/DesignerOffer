using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Designer_Offer.Infrastructure.Validations
{
    internal class UniqLoginValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                using (var context = App.Host.Services.GetRequiredService<PrimeContext>())
                {
                    if (context.UserData.AsNoTracking().Where(u => u.Login == value.ToString().Trim()).Any())
                    {
                        return new ValidationResult(false, "логин должен быть уникален");
                    }
                }
            }
            catch (Exception e)
            {
                App.Host.Services.GetRequiredService<IUserDialog>().ShowError(e.Message, "Ошибка");
            }

            return ValidationResult.ValidResult;
        }
    }
}
