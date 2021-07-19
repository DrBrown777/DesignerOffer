using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class CompanyManagerViewModel : ViewModel
    {
        #region ПОЛЯ
        private static readonly string _title = " :: Управление компанией";
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private Employee CurrentUser;
        /// <summary>
        /// Текущая компания
        /// </summary>
        private Company CurrentCompany;
        /// <summary>
        /// Сервис диалогов
        /// </summary>
        private readonly IUserDialog UserDialog;

        #region РЕПОЗИТОРИИ
        /// <summary>
        /// Репозиторий пользователей
        /// </summary>
        private readonly IRepository<Employee> RepositoryUsers;
        /// <summary>
        /// Репозиторий компаний
        /// </summary>
        private readonly IRepository<Company> RepositoryCompanies;
        /// <summary>
        /// Репозиторий должностей
        /// </summary>
        private readonly IRepository<Position> RepositoryPositions;
        /// <summary>
        /// Репозиторий разделов
        /// </summary>
        private readonly IRepository<Section> RepositorySections;
        #endregion

        #endregion

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

        private bool _Progress;
        /// <summary>
        /// Индикатор прогрессбара
        /// </summary>
        public bool Progress
        {
            get => _Progress;
            set => Set(ref _Progress, value);
        }

        private ObservableCollection<Employee> _Employees;
        /// <summary>
        /// Сотрудники
        /// </summary>
        public ObservableCollection<Employee> Employees
        {
            get => _Employees;
            set => Set(ref _Employees, value);
        }
        private Employee _SelectedEmployee;
        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
        public Employee SelectedEmployee
        {
            get => _SelectedEmployee;
            set => Set(ref _SelectedEmployee, value);
        }

        private ObservableCollection<Company> _Companies;
        /// <summary>
        /// Компании
        /// </summary>
        public ObservableCollection<Company> Companies
        {
            get => _Companies;
            set => Set(ref _Companies, value);
        }
        private Company _SelectedCompany;
        /// <summary>
        /// Выбранная компания
        /// </summary>
        public Company SelectedCompany
        {
            get => _SelectedCompany;
            set => Set(ref _SelectedCompany, value);
        }

        private ObservableCollection<Position> _Positions;
        /// <summary>
        /// Должности
        /// </summary>
        public ObservableCollection<Position> Positions
        {
            get => _Positions;
            set => Set(ref _Positions, value);
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

        private ObservableCollection<Section> _Sections;
        /// <summary>
        /// Разделы
        /// </summary>
        public ObservableCollection<Section> Sections
        {
            get => _Sections;
            set => Set(ref _Sections, value);
        }
        private Section _SelectedSection;
        /// <summary>
        /// Выбранный раздел
        /// </summary>
        public Section SelectedSection
        {
            get => _SelectedSection;
            set => Set(ref _SelectedSection, value);
        }
        #endregion

        #region КОМАНДЫ

        #region загрузка данных из репозиториев
        /// <summary>
        /// Загрузка данных из репозиториев
        /// </summary>
        public ICommand LoadDataFromRepositories { get; }

        private bool CanLoadDataFromRepositories(object p)
        {
            if (RepositoryUsers == null || RepositoryCompanies == null || RepositorySections == null || RepositoryPositions == null)
            {
                return false;
            }

            return true;
        }

        private async void OnLoadDataFromRepositories(object p)
        {
            try
            {
                Employees = new ObservableCollection<Employee>(await RepositoryUsers.Items.ToListAsync());

                //CurrentUser = Employees.SingleOrDefault(e => e.Id == App.Host.Services.GetRequiredService<IEntity>().Id);

                CurrentUser = Employees.SingleOrDefault(e => e.Id == 21);

                Status = CurrentUser.First_Name + " " + CurrentUser.Last_Name;

                CurrentCompany = CurrentUser.Company;

                Title = CurrentCompany?.Name + _title;

                Companies = new ObservableCollection<Company>(await RepositoryCompanies.Items.ToListAsync());

                Positions = new ObservableCollection<Position>(await RepositoryPositions.Items.ToListAsync());

                Sections = new ObservableCollection<Section>(await RepositorySections.Items.ToListAsync());
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

        #region добавление удаление данных
        /// <summary>
        /// Добавление новой компании
        /// </summary>
        public ICommand AddNewCompany { get; }

        private bool CanAddNewCompany(object p) => true;

        private void OnAddNewCompany(object p)
        {
            Company new_company = new Company();

            if (!UserDialog.Edit(new_company, Positions.ToList()))
            {
                return;
            }

            try
            {
                Companies.Add(RepositoryCompanies.Add(new_company));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedCompany = new_company;
            }
        }
        /// <summary>
        /// Редактирование компании
        /// </summary>
        public ICommand EditCompany { get; }

        private bool CanEditCompany(object p)
        {
            return (Company)p != null && SelectedCompany != null;
        }

        private void OnEditCompany(object p)
        {
            Company company_to_edit = (Company)p ?? SelectedCompany;

            if (!UserDialog.Edit(company_to_edit, Positions.ToList()))
            {
                return;
            }

            try
            {
                RepositoryCompanies.Update(company_to_edit);

                if (Companies.Remove(company_to_edit))
                {
                    Companies.Add(company_to_edit);
                }
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedCompany = company_to_edit;
            }
        }

        /// <summary>
        /// Удаление компании
        /// </summary>
        public ICommand RemoveCompany { get; }

        private bool CanRemoveCompany(object p)
        {
            return (Company)p != null && SelectedCompany != null;
        }

        private void OnRemoveCompany(object p)
        {
            Company company_to_remove = (Company)p ?? SelectedCompany;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить компанию {company_to_remove.Name}?", "Удаление компании"))
            {
                return;
            }

            try
            {
                RepositoryCompanies.Remove(company_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                Companies.Remove(company_to_remove);

                if (ReferenceEquals(SelectedCompany, company_to_remove))
                {
                    SelectedCompany = null;
                }
            }
        }

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        public ICommand AddNewUser { get; }

        private bool CanAddNewUser(object p) => true;

        private void OnAddNewUser(object p)
        {
            UserData new_user = new UserData();

            Employee new_employee = new Employee
            {
                UserData = new_user
            };

            if (!UserDialog.Edit(new_employee, Companies.ToList()))
            {
                return;
            }

            try
            {
                Employees.Add(RepositoryUsers.Add(new_employee));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedEmployee = new_employee;
            }
        }
        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        public ICommand EditUser { get; }

        private bool CanEditUser(object p)
        {
            return true && (Employee)p != null && SelectedEmployee != null;
        }

        private void OnEditUser(object p)
        {
            Employee employee = (Employee)p ?? SelectedEmployee;

            if (!UserDialog.Edit(employee, Companies.ToList()))
            {
                return;
            }

            try
            {
                RepositoryUsers.Update(employee);

                if (Employees.Remove(employee))
                {
                    Employees.Add(employee);
                }
            }
            catch (Exception e)
            {

                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedEmployee = employee;
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        public ICommand RemoveUser { get; }

        private bool CanRemoveUser(object p)
        {
            return (Employee)p != null && SelectedEmployee != null;
        }

        private void OnRemoveUser(object p)
        {
            Employee user_to_remove = (Employee)p ?? SelectedEmployee;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить сотрудника {user_to_remove.Last_Name}?", "Удаление сотрудника"))
            {
                return;
            }

            try
            {
                RepositoryUsers.Remove(user_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                Employees.Remove(user_to_remove);

                if (ReferenceEquals(SelectedEmployee, user_to_remove))
                {
                    SelectedEmployee = null;
                }
            }
        }
        /// <summary>
        /// Добавление новой должности
        /// </summary>
        public ICommand AddNewPosition { get; }

        private bool CanAddNewPosition(object p) => true;

        private void OnAddNewPosition(object p)
        {
            Position new_position = new Position();
            
            if (!UserDialog.Edit(new_position))
            {
                return;
            }

            try
            {
                Positions.Add(RepositoryPositions.Add(new_position));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Oшибка");
            }
            finally
            {
                SelectedPosition = new_position;
            }
        }
        /// <summary>
        /// Редактирование должности
        /// </summary>
        public ICommand EditPosition { get; }

        private bool CanEditPosition(object p)
        {
            return true && SelectedPosition != null && (Position)p != null;
        }

        private void OnEditPosition(object p)
        {
            Position position = SelectedPosition ?? (Position)p;

            if (!UserDialog.Edit(position))
            {
                return;
            }

            try
            {
                RepositoryPositions.Update(position);

                if (Positions.Remove(position))
                {
                    Positions.Add(position);
                }
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedPosition = position;
            }
        }
        /// <summary>
        /// Удаление должности
        /// </summary>
        public ICommand RemovePosition { get; }

        private bool CanRemovePosition(object p)
        {
            return true && SelectedPosition != null && (Position)p != null;
        }

        private void OnRemovePosition(object p)
        {
            Position position_to_remove = SelectedPosition ?? (Position)p;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить должность {position_to_remove.Name}?", "Удаление должности"))
            {
                return;
            }

            try
            {
                RepositoryPositions.Remove(position_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                Positions.Remove(position_to_remove);

                if (ReferenceEquals(SelectedPosition, position_to_remove))
                {
                    SelectedPosition = null;
                }
            }
        }

        #endregion
        #endregion

        #region КОНСТРУКТОРЫ
        public CompanyManagerViewModel(
            IRepository<Employee> repaUser,
            IRepository<Company> repaCompany,
            IRepository<Position> repaPosition,
            IRepository<Section> repaSections,
            IUserDialog userDialog)
        {
            Progress = true;

            UserDialog = userDialog;

            RepositoryUsers = repaUser;
            RepositoryCompanies = repaCompany;
            RepositoryPositions = repaPosition;
            RepositorySections = repaSections;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);

            AddNewCompany = new LambdaCommand(OnAddNewCompany, CanAddNewCompany);
            EditCompany = new LambdaCommand(OnEditCompany, CanEditCompany);
            RemoveCompany = new LambdaCommand(OnRemoveCompany, CanRemoveCompany);

            AddNewUser = new LambdaCommand(OnAddNewUser, CanAddNewUser);
            EditUser = new LambdaCommand(OnEditUser, CanEditUser);
            RemoveUser = new LambdaCommand(OnRemoveUser, CanRemoveUser);

            AddNewPosition = new LambdaCommand(OnAddNewPosition, CanAddNewPosition);
            EditPosition = new LambdaCommand(OnEditPosition, CanEditPosition);
            RemovePosition = new LambdaCommand(OnRemovePosition, CanRemovePosition);
        }
        #endregion
    }
}