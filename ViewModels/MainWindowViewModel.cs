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
        #region ПОЛЯ

        /// <summary>
        /// Интерфейс страницы Логин
        /// </summary>
        private readonly ILoginViewModel LoginView;

        /// <summary>
        /// Интерфейс страницы Регистрации
        /// </summary>
        private readonly IRegistrationViewModel RegistrationView;

        /// <summary>
        /// Список компаний
        /// </summary>
        private List<Company> Companies;
        #endregion

        #region СВОЙСТВА

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
            return LoginView != null;
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
            return RegistrationView != null;
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

                    LoginView.Update(Companies);
                    RegistrationView.Update(Companies);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ

        public MainWindowViewModel(ILoginViewModel loginView, IRegistrationViewModel registrationView)
        {
            LoginView = loginView; 
            RegistrationView = registrationView;

            LoadLoginPage = new LambdaCommand(OnLoadLoginPage, CanLoadLoginPage);
            LoadRegistarationPage = new LambdaCommand(OnLoadRegistarationPage, CanLoadRegistarationPage);

            LoginView.LoadRegistarationPageCommand = LoadRegistarationPage;
            RegistrationView.LoadLoginPageCommand = LoadLoginPage;

            GetAllCompanies();

            LoadLoginPage.Execute(null);
        }
        #endregion
    }
}
