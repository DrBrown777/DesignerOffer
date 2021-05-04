﻿using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Data;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
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
            AnyViewModel = LoginPage;
        }

        private bool CanLoadLoginPage(object p)
        {
            return true && LoginPage != null || AnyViewModel != null;
        }

        /// <summary>
        /// Команда загрузки страницы Регистрации
        /// </summary>
        public ICommand LoadRegistarationPage { get; }

        private void OnLoadRegistarationPage(object p)
        {
            AnyViewModel = RegistrationPage;
        }

        private bool CanLoadRegistarationPage(object p)
        {
            return true && RegistrationPage != null || AnyViewModel != null;
        }

        public MainWindowViewModel()
        {
            LoadLoginPage = new LambdaCommand(OnLoadLoginPage, CanLoadLoginPage);
            LoadRegistarationPage = new LambdaCommand(OnLoadRegistarationPage, CanLoadRegistarationPage);

            LoginPage = new LoginViewModel(LoadRegistarationPage);
            RegistrationPage = new RegistrationViewModel(LoadLoginPage);

            AnyViewModel = LoginPage;
        }
    }
}
