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
    internal class UniqEmailValidationRule : ValidationRule
    {
        private static readonly string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                using (var context = App.Host.Services.GetRequiredService<PrimeContext>())
                {
                    if (!Regex.IsMatch(value.ToString(), pattern))
                    {
                        return new ValidationResult(false, "не валидный email");
                    }
                    else if (context.Employee.AsNoTracking().Where(e => e.Mail == value.ToString().Trim()).Any())
                    {
                        return new ValidationResult(false, "email должен быть уникален");
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
