using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Designer_Offer.Infrastructure.Validations
{
    internal class UniqPhoneValidationRule : ValidationRule
    {
        private static readonly PrimeContext context;
        private static readonly IUserDialog userDialog;

        private static readonly string pattern = @"^((\+?3)?8)?0\d{9}$";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (!Regex.IsMatch(value.ToString(), pattern))
                {
                    return new ValidationResult(false, "формат номера +380675552233");
                }
                else if (context.Employee.AsNoTracking().Where(e => e.Phone == value.ToString().Trim()).Any())
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
