using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Designer_Offer.Infrastructure.Validations
{
    internal class UniqPhoneValidationRule : ValidationRule
    {
        private static readonly PrimeContext context;
        private static readonly IUserDialog userDialog;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (context.Employees.AsNoTracking().Where(e => e.Phone == value.ToString().Trim()).Any())
                {
                    return new ValidationResult(false, "телефон должен быть уникален");
                }
            }
            catch (Exception e)
            {
                userDialog.ShowError(e.Message, "Ошибка");
            }

            return ValidationResult.ValidResult;
        }

        static UniqPhoneValidationRule()
        {
            context = App.Host.Services.GetRequiredService<PrimeContext>();
            userDialog = App.Host.Services.GetRequiredService<IUserDialog>();
        }
    }
}
