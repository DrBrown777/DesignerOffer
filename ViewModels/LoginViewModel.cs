using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
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
        #region СВОЙСТВА
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

        /// <summary>
        /// Пользователь
        /// </summary>
        private UserData User;

        //private List<Company> _Companies;
        /// <summary>
        /// Список компаний
        /// </summary>
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

        private Company _SelectedComapany;
        /// <summary>
        /// Выбранная компания
        /// </summary>
        public Company SelectedCompany
        {
            get => _SelectedComapany;
            set => Set(ref _SelectedComapany, value);
        }
        #endregion

        #region КОМАНДЫ
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
                var work = App.Host.Services.GetRequiredService<WorkWindow>();

                Application.Current.MainWindow.Close();

                Application.Current.MainWindow = work;

                work.Show();
            }
        }

        private bool CanLoginCommand(object p)
        {
            if (string.IsNullOrWhiteSpace(((PasswordBox)p).Password))
            {
                return false;
            }
            else if (SelectedCompany == null)
                return false;
            else
                return true;
        }
        #endregion

        #region МЕТОДЫ
        private bool LoginSucces(PasswordBox passBox)
        {
            try
            {
                User = (from u in contextDB.UserData.AsNoTracking()
                        join e in contextDB.Employee on u.Employee_Id equals e.Id
                        join c in contextDB.Company on e.Company_Id equals SelectedCompany.Id
                        select u).FirstOrDefault(u => u.Login == Login);
            }
            catch(Exception e)
            {
                Status = e.Message;
            }
           
            if (User == null || User.Password != passBox.Password.Trim().ToLower())
            {
                Status = "Неправильный логин или пароль!";

                Timer timer = new Timer(new TimerCallback(ChangeStatus), 
                                        "Для входа в систему введите Логин и Пароль", 1500, 0);
                return false;
            }
            return true;
        }

        private void ChangeStatus(object p)
        {
            Status = p.ToString();
        }

        public void Update(List<Company> companies)
        {
            if (Equals(companies, Companies)) return;
            Companies = companies;
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public LoginViewModel(){}

        public LoginViewModel(ICommand loadregister)
        {
            LoginCommand = new LambdaCommand(OnLoginCommand, CanLoginCommand);
            LoadRegistarationPageCommand = loadregister;

            Status = "Для входа в систему введите Логин и Пароль";
            Title = "Вход в систему";
        }
        #endregion
    }
}
