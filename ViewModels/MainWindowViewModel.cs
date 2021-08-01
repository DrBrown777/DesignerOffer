using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Data;
using System.Windows.Input;
using System;
using System.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Designer_Offer.Views.Pages;
using System.Threading.Tasks;
using Designer_Offer.Services;
using Designer_Offer.Services.Interfaces;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region ПОЛЯ
        /// <summary>
        /// Интерфейс страницы Логин
        /// </summary>
        private readonly ILoginService LoginView;

        /// <summary>
        /// Интерфейс страницы Регистрации
        /// </summary>
        private readonly IRegistrationService RegistrationView;

        /// <summary>
        /// Интерфейс окна диалога
        /// </summary>
        private readonly IUserDialog UserDialog;
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

        private bool _Progress;
        /// <summary>
        /// Индикатор прогрессбара
        /// </summary>
        public bool Progress
        {
            get => _Progress;
            set => Set(ref _Progress, value);
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
        private async void UpdateAllPages(IRepository<Company> companyRepository)
        {
            try
            {
                var Company = await companyRepository.Items.AsNoTracking().ToListAsync();

                LoginView.Update(Company);
                RegistrationView.Update(Company);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                Progress = false;
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public MainWindowViewModel(
            IRepository<Company> companyRepository,
            ILoginService loginView,
            IRegistrationService registrationView,
            IUserDialog userDialog)
        {
            Progress = true;

            UserDialog = userDialog;

            LoginView = loginView; 
            RegistrationView = registrationView;

            LoadLoginPage = new LambdaCommand(OnLoadLoginPage, CanLoadLoginPage);
            LoadRegistarationPage = new LambdaCommand(OnLoadRegistarationPage, CanLoadRegistarationPage);

            LoginView.LoadRegistarationPageCommand = LoadRegistarationPage;
            RegistrationView.LoadLoginPageCommand = LoadLoginPage;

            Task.Run(() => UpdateAllPages(companyRepository));

            LoadLoginPage.Execute(null);
        }
        #endregion
    }
}
