using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Data;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Designer_Offer.Views.Pages;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region СВОЙСТВА
        private List<Company> _Companies;
        /// <summary>
        /// Список компаний
        /// </summary>
        private List<Company> Companies
        {
            get => _Companies;
            set
            {
                Set(ref _Companies, value);
            }
        }

        private Page _AnyPage;
        /// <summary>
        /// Любая страница во Frame
        /// </summary>
        public Page AnyPage
        {
            get => _AnyPage;
            set => Set(ref _AnyPage, value);
        }
        #endregion

        #region КОМАНДЫ
        /// <summary>
        /// Команда загрузки страницы Логина
        /// </summary>
        public ICommand LoadLoginPage { get; }

        private void OnLoadLoginPage(object p)
        {
            AnyPage = App.Host.Services.GetRequiredService<Login>();
        }

        private bool CanLoadLoginPage(object p)
        {
            return true;
        }

        /// <summary>
        /// Команда загрузки страницы Регистрации
        /// </summary>
        public ICommand LoadRegistarationPage { get; }

        private void OnLoadRegistarationPage(object p)
        {
            AnyPage = App.Host.Services.GetRequiredService<Registration>();
        }

        private bool CanLoadRegistarationPage(object p)
        {
            return true;
        }
        #endregion

        #region МЕТОДЫ
        private async void GetAllCompanies()
        {
            try
            {
                using (var context = new PrimeContext())
                {
                    Companies = await context.Company.AsNoTracking().ToListAsync();
                    App.Host.Services.GetRequiredService<LoginViewModel>().Update(Companies);
                    App.Host.Services.GetRequiredService<RegistrationViewModel>().Update(Companies);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdatePage()
        {
            App.Host.Services.GetRequiredService<LoginViewModel>().LoadRegistarationPageCommand = LoadRegistarationPage;
            App.Host.Services.GetRequiredService<RegistrationViewModel>().LoadLoginPageCommand = LoadLoginPage;
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public MainWindowViewModel()
        {
            LoadLoginPage = new LambdaCommand(OnLoadLoginPage, CanLoadLoginPage);
            LoadRegistarationPage = new LambdaCommand(OnLoadRegistarationPage, CanLoadRegistarationPage);

            UpdatePage();

            GetAllCompanies();

            LoadLoginPage.Execute(null);
        }
        #endregion
    }
}
