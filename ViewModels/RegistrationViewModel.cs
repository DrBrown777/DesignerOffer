using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Windows.Controls;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class RegistrationViewModel : ViewModel
    {
        private string _UserLogin;
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserLogin
        {
            get => _UserLogin;
            set => Set(ref _UserLogin, value);
        }

        private string _UserName;
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName
        {
            get => _UserName;
            set => Set(ref _UserName, value);
        }

        private string _UserSurName;
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string UserSurName
        {
            get => _UserSurName;
            set => Set(ref _UserSurName, value);
        }

        private string _UserEmail;
        /// <summary>
        /// E-mail пользователя
        /// </summary>
        public string UserEmail
        {
            get => _UserEmail;
            set => Set(ref _UserEmail, value);
        }

        private string _UserPhone;
        /// <summary>
        /// Телефон пользователя
        /// </summary>
        public string UserPhone
        {
            get => _UserPhone;
            set => Set(ref _UserPhone, value);
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

        private List<Position> _Positions;
        /// <summary>
        /// Список должностей
        /// </summary>
        public List<Position> Positions
        {
            get => _Positions;
            set => Set(ref _Positions, value);
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

        private string _SelectedPosition;
        /// <summary>
        /// Выбранная должность
        /// </summary>
        public string SelectedPosition
        {
            get => _SelectedPosition;
            set => Set(ref _SelectedPosition, value);
        }

        /// <summary>
        /// Команда загрузки страницы Логина
        /// </summary>
        public ICommand LoadLoginPageCommand { get; }

        public ICommand LoadPositionCommand { get; }
        /// <summary>
        /// Команда заполняет должности выбранной компании
        /// </summary>
        private void OnLoadPositionCommand(object p)
        {
            int IdSelectedCompany = Convert.ToInt32(SelectedCompany);

            Positions = (from pos in contextDB.Position
                   join cpPos in contextDB.CompanyPosition on pos.Id equals cpPos.Position_Id
                   where cpPos.Company_Id.Equals(IdSelectedCompany)
                   select pos).ToList();
        }

        private bool CanLoadPositionCommand(object p)
        {
            return true && contextDB != null;
        }

        public ICommand RegistrationCommand { get; }

        private void OnRegistrationCommand(object p)
        {
            PasswordBox passBox = (PasswordBox)p;

            UserData user = new UserData()
            {
                Login = UserLogin,
                Password = passBox.Password
            };

            Employee employee = new Employee()
            {
                First_Name = UserName,
                Last_Name = UserSurName,
                Mail = UserEmail,
                Phone = UserPhone,
                Company_Id = Convert.ToInt32(SelectedCompany),
                Position_Id = Convert.ToInt32(SelectedPosition)
            };

            employee.UserData.Add(user);

            contextDB.Employee.Add(employee);

            contextDB.SaveChanges();
        }

        private bool CanRegistrationCommand(object p)
        {
            return true && SelectedCompany != null && SelectedPosition != null;
        }

        public RegistrationViewModel(ICommand loadlogin)
        {
            LoadLoginPageCommand = loadlogin;
            LoadPositionCommand = new LambdaCommand(OnLoadPositionCommand, CanLoadPositionCommand);
            RegistrationCommand = new LambdaCommand(OnRegistrationCommand, CanRegistrationCommand);

            if (contextDB != null)
                Companies = contextDB.Company.ToList();

            Status = "Для регистрации заполните все поля";
            Title = "Designer Offer :: Регистрация в системе";
        }
    }
}
