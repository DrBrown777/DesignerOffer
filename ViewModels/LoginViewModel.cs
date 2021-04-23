using Designer_Offer.Data;
using Designer_Offer.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class LoginViewModel : ViewModel
    {
        private List<Company> _Companies;
        /// <summary>
        /// Список компаний
        /// </summary>
        public List<Company> Companies
        {
            get => _Companies;
            set => Set(ref _Companies, value);
        }

        private string _Login;
        /// <summary>
        /// Поле Логин
        /// </summary>
        public string Login
        {
            get => _Login;
            set => Set(ref _Login, value);
        }
        /// <summary>
        /// Команда загрузки страницы Регистрации
        /// </summary>
        public ICommand LoadRegistarationPage { get; }

        public LoginViewModel(ICommand loadregister)
        {
            if (contextDB != null) 
                Companies = contextDB.Company.ToList();

            LoadRegistarationPage = loadregister;
            Status = "Для входа в систему введите Логин и Пароль";
            Title = "Designer Offer :: Вход в систему";
        }
    }
}
