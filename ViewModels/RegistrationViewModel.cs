using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class RegistrationViewModel : ViewModel
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

        private Company _SelectedComapany;
        /// <summary>
        /// Выбранная компания
        /// </summary>
        public Company SelectedCompany
        {
            get => _SelectedComapany;
            set => Set(ref _SelectedComapany, value);
        }

        private Position _SelectedPosition;
        /// <summary>
        /// Выбранная должность
        /// </summary>
        public Position SelectedPosition
        {
            get => _SelectedPosition;
            set => Set(ref _SelectedPosition, value);
        }
        #endregion

        #region КОМАНДЫ
        /// <summary>
        /// Команда загрузки страницы Логина
        /// </summary>
        public ICommand LoadLoginPageCommand { get; set; }

        /// <summary>
        /// Команда заполняет должности выбранной компании
        /// </summary>
        public ICommand LoadPositionCommand { get; }
        
        private async void OnLoadPositionCommand(object p)
        {
            try
            {
                using (var context = new PrimeContext())
                {
                    Positions = await (from pos in context.Position
                                       join cpPos in context.CompanyPosition on pos.Id equals cpPos.Position_Id
                                       where cpPos.Company_Id.Equals(SelectedCompany.Id)
                                       select pos).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception e)
            {
                Status = e.Message;
            }
        }

        private bool CanLoadPositionCommand(object p)
        {
            return true && SelectedCompany != null;
        }
        /// <summary>
        /// Команда проводит регистрацию
        /// </summary>
        public ICommand RegistrationCommand { get; }

        private async void OnRegistrationCommand(object p)
        {
            PasswordBox passBox = (PasswordBox)p;

            UserData user = new UserData()
            {
                Login = UserLogin,
                Password = passBox.Password.Trim()
            };

            Employee employee = new Employee()
            {
                First_Name = UserName,
                Last_Name = UserSurName,
                Mail = UserEmail,
                Phone = UserPhone,
                Company_Id = SelectedCompany.Id,
                Position_Id = SelectedPosition.Id
            };

            employee.UserData.Add(user);

            try
            {
                using (var context = new PrimeContext())
                {
                    context.Employee.Add(employee);

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Status = e.Message;
            }
            finally
            {
                LoadLoginPageCommand.Execute(null);

                MessageBox.Show("Ваш аккаунт зарегистрован!\nТеперь вы можете войти.", 
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region МЕТОДЫ
        private bool CanRegistrationCommand(object p)
        {
            if (SelectedCompany == null || SelectedPosition == null)
                return false;
            else if (string.IsNullOrWhiteSpace(((PasswordBox)p).Password))
                return false;
            else
                return true;
        }

        public void Update(List<Company> companies)
        {
            if (Equals(companies, Companies)) return;
            Companies = companies;
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public RegistrationViewModel()
        {
            LoadPositionCommand = new LambdaCommand(OnLoadPositionCommand, CanLoadPositionCommand);
            RegistrationCommand = new LambdaCommand(OnRegistrationCommand, CanRegistrationCommand);

            Status = "Для регистрации заполните все поля";
            Title = "Регистрация в системе";
        }
        #endregion
    }
}
