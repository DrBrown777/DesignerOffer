using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Data;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Data.Entity;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
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
                LoginPageViewModel.Update(_Companies);
            }
        }

        /// <summary>
        /// ViewModel страницы Логина
        /// </summary>
        private LoginViewModel LoginPageViewModel;

        /// <summary>
        /// ViewModel страницы Регистрации
        /// </summary>
        private RegistrationViewModel RegistrationPageViewModel;

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
            LoginPageViewModel = new LoginViewModel(LoadRegistarationPage);
            LoginPageViewModel.Update(Companies);
           
            AnyViewModel = LoginPageViewModel;

            RegistrationPageViewModel = null;
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
            RegistrationPageViewModel = new RegistrationViewModel(LoadLoginPage);
            RegistrationPageViewModel.Update(Companies);

            AnyViewModel = RegistrationPageViewModel;

            LoginPageViewModel = null;
        }

        private bool CanLoadRegistarationPage(object p)
        {
            return RegistrationPageViewModel is null;
        }

        private async void GetCompanies()
        {
            try
            {
                if (contextDB != null)
                    Companies = await contextDB.Company.ToListAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public MainWindowViewModel()
        {
            LoadLoginPage = new LambdaCommand(OnLoadLoginPage, CanLoadLoginPage);
            LoadRegistarationPage = new LambdaCommand(OnLoadRegistarationPage, CanLoadRegistarationPage);
            
            GetCompanies();
            LoadLoginPage.Execute(null);
        }
    }
}
