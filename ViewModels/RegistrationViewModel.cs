using Designer_Offer.ViewModels.Base;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class RegistrationViewModel : ViewModel
    {
        private string _Title;
        /// <summary>
        /// Заголовок Окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        /// <summary>
        /// Команда загрузки страницы Логина
        /// </summary>
        public ICommand LoadLoginPage { get; }

        public RegistrationViewModel(ICommand loadlogin)
        {
            LoadLoginPage = loadlogin;
        }
    }
}
