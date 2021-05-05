using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class LoginViewModel : ViewModel
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

        private string _Status;
        /// <summary>
        /// Статус программы
        /// </summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

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

        private string _SelectedComapany;
        /// <summary>
        /// Выбранная компания
        /// </summary>
        public string SelectedCompany
        {
            get => _SelectedComapany;
            set => Set(ref _SelectedComapany, value);
        }
        /// <summary>
        /// Команда загрузки страницы Регистрации
        /// </summary>
        public ICommand LoadRegistarationPageCommand { get; }

        public ICommand LoginCommand { get; }
        /// <summary>
        /// Команда Входим в систему
        /// </summary>
        private void OnLoginCommand(object p)
        {
            if (LoginSucces((PasswordBox)p))
            {
                WorkWindow work = new WorkWindow();

                work.Show();

                Application.Current.MainWindow.Close();
            }
        }

        private bool CanLoginCommand(object p)
        {
            PasswordBox passBox = (PasswordBox)p;
            string pass = passBox.Password;

            if (string.IsNullOrWhiteSpace(pass))
            {
                return false;
            }
            else if (SelectedCompany == null)
                return false;
            else
                return true;
        }

        private bool LoginSucces(PasswordBox passBox)
        {
            int id = Convert.ToInt32(SelectedCompany);

            string pass = passBox.Password.Trim().ToLower();
            
            UserData userData = contextDB.UserData.FirstOrDefault(u => u.Login == Login);

            if (userData == null || userData.Password != pass)
            {
                Status = "Неправильный логин или пароль!";

                TimerCallback callback = new TimerCallback(ChangeStatus);
                Timer timer = new Timer(callback, "Для входа в систему введите Логин и Пароль", 1500, 0);
                
                return false;
            }

            return true;
        }

        private void ChangeStatus(object p)
        {
            Status = p.ToString();
        }

        public LoginViewModel(ICommand loadregister)
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

            LoginCommand = new LambdaCommand(OnLoginCommand, CanLoginCommand);
            LoadRegistarationPageCommand = loadregister;

            Status = "Для входа в систему введите Логин и Пароль";
            Title = "Вход в систему";
        }
    }
}
