using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Data;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System;
using System.Windows.Controls;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        /// <summary>
        /// Список компаний
        /// </summary>
        private static readonly List<Company> Companies;

        /// <summary>
        /// Страница Логина
        /// </summary>
        protected Page LoginPage;
        /// <summary>
        /// ViewModel страницы Логина
        /// </summary>
        private ViewModel LoginPageViewModel;

        /// <summary>
        /// Cтраница Регистрации
        /// </summary>
        protected Page RegistrationPage;
        /// <summary>
        /// ViewModel страницы Регистрации
        /// </summary>
        private ViewModel RegistrationPageViewModel;

        private ViewModel _AnyViewModel;
        /// <summary>
        /// Любая страница во Frame
        /// </summary>
        public ViewModel AnyViewModel
        {
            get => _AnyViewModel;
            set => Set(ref _AnyViewModel, value);
        }

        /// <summary>
        /// Команда загрузки страницы Логина
        /// </summary>
        public ICommand LoadLoginPage { get; }

        private void OnLoadLoginPage(object p)
        {
            LoginPageViewModel = new LoginViewModel(LoadRegistarationPage, Companies);
            LoginPage = new Page();
           
            AnyViewModel = LoginPageViewModel;

            RegistrationPageViewModel = null;
            RegistrationPage = null;
        }

        private bool CanLoadLoginPage(object p)
        {
            return LoginPageViewModel is null;
        }

        /// <summary>
        /// Команда загрузки страницы Регистрации
        /// </summary>
        public ICommand LoadRegistarationPage { get; }

        private void OnLoadRegistarationPage(object p)
        {
            RegistrationPageViewModel = new RegistrationViewModel(LoadLoginPage, Companies);
            RegistrationPage = new Page();

            AnyViewModel = RegistrationPageViewModel;

            LoginPageViewModel = null;
            LoginPage = null;
        }

        private bool CanLoadRegistarationPage(object p)
        {
            return RegistrationPageViewModel is null;
        }

        static MainWindowViewModel()
        {
            try
            {
                if (contextDB != null)
                    Companies = contextDB.Company.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка соединения с базой данных\n" + e.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public MainWindowViewModel()
        {
            LoadLoginPage = new LambdaCommand(OnLoadLoginPage, CanLoadLoginPage);
            LoadRegistarationPage = new LambdaCommand(OnLoadRegistarationPage, CanLoadRegistarationPage);

            LoadLoginPage.Execute(null);
        }
    }
}
