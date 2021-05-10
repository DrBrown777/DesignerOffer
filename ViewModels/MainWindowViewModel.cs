using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Data;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Data.Entity;
using Designer_Offer.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace Designer_Offer.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region СВОЙСТВА
        private readonly IDataService DataService;

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
                UpdatePages();
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
        #endregion

        #region КОМАНДЫ
        /// <summary>
        /// Команда загрузки страницы Логина
        /// </summary>
        public ICommand LoadLoginPage { get; }

        private void OnLoadLoginPage(object p)
        {
            AnyViewModel = LoginPageViewModel;
        }

        private bool CanLoadLoginPage(object p)
        {
            return LoginPageViewModel != null;
        }

        /// <summary>
        /// Команда загрузки страницы Регистрации
        /// </summary>
        public ICommand LoadRegistarationPage { get; }

        private void OnLoadRegistarationPage(object p)
        {
            AnyViewModel = RegistrationPageViewModel;
        }

        private bool CanLoadRegistarationPage(object p)
        {
            return RegistrationPageViewModel != null;
        }
        #endregion

        #region МЕТОДЫ
        private async Task GetAllCompaniesAsync()
        {
            try
            {
                var comp = await DataService.GetAllAsync();
                Companies = comp;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdatePages()
        {
            LoginPageViewModel.Update(Companies, LoadRegistarationPage);
            RegistrationPageViewModel.Update(Companies, LoadLoginPage);
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public MainWindowViewModel(LoginViewModel loginViewModel, RegistrationViewModel registrationViewModel, IDataService dataService)
        {
            DataService = dataService;

            LoginPageViewModel = loginViewModel;
            RegistrationPageViewModel = registrationViewModel;

            LoadLoginPage = new LambdaCommand(OnLoadLoginPage, CanLoadLoginPage);
            LoadRegistarationPage = new LambdaCommand(OnLoadRegistarationPage, CanLoadRegistarationPage);

            AnyViewModel = LoginPageViewModel;

            _ = GetAllCompaniesAsync();
        }
        #endregion
    }
}
