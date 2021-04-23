using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        /// <summary>
        /// ViewModel страницы Логина
        /// </summary>
        private readonly ViewModel LoginPage;

        /// <summary>
        /// ViewModel страницы Регистрации
        /// </summary>
        private readonly ViewModel RegistrationPage;

        private ViewModel _AnyViewModel;
        /// <summary>
        /// Любая страница во Frame
        /// </summary>
        public ViewModel AnyViewModel
        {
            get => _AnyViewModel;
            set => Set(ref _AnyViewModel, value);
        }

        private string _Title = "Designer Offer";
        /// <summary>
        /// Заголовок Окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private string _Status = "Для входа в систему введите Логин и Пароль";
        /// <summary>
        /// Статус программы
        /// </summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

        /// <summary>
        /// Команда загрузки страницы Логина
        /// </summary>
        public ICommand LoadLoginPage { get; }

        private void OnLoadLoginPage(object p) => AnyViewModel = LoginPage;

        private bool CanLoadLoginPage(object p)
        {
            if (LoginPage != null || AnyViewModel != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Команда загрузки страницы Регистрации
        /// </summary>
        public ICommand LoadRegistarationPage { get; }

        private void OnLoadRegistarationPage(object p) => AnyViewModel = RegistrationPage;

        private bool CanLoadRegistarationPage(object p)
        {
            if (RegistrationPage != null || AnyViewModel != null)
                return true;
            else
                return false;
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
