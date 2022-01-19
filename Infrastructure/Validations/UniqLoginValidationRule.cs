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
        private static readonly PrimeContext context;
        private static readonly IUserDialog userDialog;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (context.UsersData.AsNoTracking().Where(u => u.Login == value.ToString().Trim()).Any())
                {
                    return new ValidationResult(false, "логин должен быть уникален");
                }
            }
            catch (Exception e)
            {
                userDialog.ShowError(e.Message, "Ошибка");
            }

            return ValidationResult.ValidResult;
        }

        static UniqLoginValidationRule()
        {
            context = App.Host.Services.GetRequiredService<PrimeContext>();
            userDialog = App.Host.Services.GetRequiredService<IUserDialog>();
        }
    }
}
