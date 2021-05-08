using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Data;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        /// <summary>
        /// Список компаний
        /// </summary>
        private static readonly List<Company> Companies;

        /// <summary>
        /// ViewModel страницы Логина
        /// </summary>
        private static ViewModel LoginPage;

        /// <summary>
        /// ViewModel страницы Регистрации
        /// </summary>
        private static ViewModel RegistrationPage;

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
            LoginPage = new LoginViewModel(LoadRegistarationPage, Companies);
            AnyViewModel = LoginPage;
            RegistrationPage = null;
        }

        private bool CanLoadLoginPage(object p)
        {
            return LoginPage is null;
        }

        /// <summary>
        /// Команда загрузки страницы Регистрации
        /// </summary>
        public ICommand LoadRegistarationPage { get; }

        private void OnLoadRegistarationPage(object p)
        {
            RegistrationPage = new RegistrationViewModel(LoadLoginPage, Companies);
            AnyViewModel = RegistrationPage;
            LoginPage = null;
        }

        private bool CanLoadRegistarationPage(object p)
        {
            return RegistrationPage is null;
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
