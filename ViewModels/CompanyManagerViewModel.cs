using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
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
        private Employees CurrentUser;

        /// <summary>
        /// Текущая компания
        /// </summary>
        private Companies CurrentCompany;

        /// <summary>
        /// Сервис диалогов
        /// </summary>
        private readonly IUserDialog UserDialog;

        #region РЕПОЗИТОРИИ
        /// <summary>
        /// Репозиторий пользователей
        /// </summary>
        private readonly IRepository<Employees> RepositoryUsers;
        /// <summary>
        /// Репозиторий компаний
        /// </summary>
        private readonly IRepository<Companies> RepositoryCompanies;
        /// <summary>
        /// Репозиторий должностей
        /// </summary>
        private readonly IRepository<Positions> RepositoryPositions;
        /// <summary>
        /// Репозиторий разделов
        /// </summary>
        private readonly IRepository<Sections> RepositorySections;
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

        private ObservableCollection<Employees> _Employees;
        /// <summary>
        /// Сотрудники
        /// </summary>
        public ObservableCollection<Employees> Employees
        {
            get => _Employees;
            set => Set(ref _Employees, value);
        }

        private Employees _SelectedEmployee;
        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
        public Employees SelectedEmployee
        {
            get => _SelectedEmployee;
            set => Set(ref _SelectedEmployee, value);
        }

        private ObservableCollection<Companies> _Companies;
        /// <summary>
        /// Компании
        /// </summary>
        public ObservableCollection<Companies> Companies
        {
            get => _Companies;
            set => Set(ref _Companies, value);
        }

        private Companies _SelectedCompany;
        /// <summary>
        /// Выбранная компания
        /// </summary>
        public Companies SelectedCompany
        {
            get => _SelectedCompany;
            set => Set(ref _SelectedCompany, value);
        }

        private ObservableCollection<Positions> _Positions;
        /// <summary>
        /// Должности
        /// </summary>
        public ObservableCollection<Positions> Positions
        {
            get => _Positions;
            set => Set(ref _Positions, value);
        }

        private Positions _SelectedPosition;
        /// <summary>
        /// Выбранная должность
        /// </summary>
        public Positions SelectedPosition
        {
            get => _SelectedPosition;
            set => Set(ref _SelectedPosition, value);
        }

        private ObservableCollection<Sections> _Sections;
        /// <summary>
        /// Разделы
        /// </summary>
        public ObservableCollection<Sections> Sections
        {
            get => _Sections;
            set => Set(ref _Sections, value);
        }

        private Sections _SelectedSection;
        /// <summary>
        /// Выбранный раздел
        /// </summary>
        public Sections SelectedSection
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
                Employees = new ObservableCollection<Employees>(await RepositoryUsers.Items.ToListAsync());

                //CurrentUser = Employees.SingleOrDefault(e => e.Id == App.Host.Services.GetRequiredService<Employee>().Id);

                CurrentUser = Employees.SingleOrDefault(e => e.Id == 21);

                Status = CurrentUser.First_Name + " " + CurrentUser.Last_Name;

                CurrentCompany = CurrentUser.Companies;

                Title = CurrentCompany?.Name + _title;

                Companies = new ObservableCollection<Companies>(await RepositoryCompanies.Items.ToListAsync());

                Positions = new ObservableCollection<Positions>(await RepositoryPositions.Items.ToListAsync());

                Sections = new ObservableCollection<Sections>(await RepositorySections.Items.ToListAsync());
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
            Companies new_company = new Companies();

            if (!UserDialog.Edit(new_company))
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
            return (Companies)p != null && SelectedCompany != null;
        }

        private void OnEditCompany(object p)
        {
            Companies company_to_edit = (Companies)p ?? SelectedCompany;

            if (!UserDialog.Edit(company_to_edit))
            {
                return;
            }

            try
            {
                RepositoryCompanies.Update(company_to_edit);
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
            return (Companies)p != null && SelectedCompany != null;
        }

        private void OnRemoveCompany(object p)
        {
            Companies company_to_remove = (Companies)p ?? SelectedCompany;

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
            UsersData new_user = new UsersData();

            Employees new_employee = new Employees
            {
                UsersData = new_user
            };

            if (!UserDialog.Edit(new_employee))
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
            return true && (Employees)p != null && SelectedEmployee != null;
        }

        private void OnEditUser(object p)
        {
            Employees employee = (Employees)p ?? SelectedEmployee;

            if (!UserDialog.Edit(employee))
            {
                return;
            }

            try
            {
                RepositoryUsers.Update(employee);

                /*убрать*/
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
            return (Employees)p != null && SelectedEmployee != null;
        }

        private void OnRemoveUser(object p)
        {
            Employees user_to_remove = (Employees)p ?? SelectedEmployee;

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
            Positions new_position = new Positions();
            
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
            return true && SelectedPosition != null && (Positions)p != null;
        }

        private void OnEditPosition(object p)
        {
            Positions position = SelectedPosition ?? (Positions)p;

            if (!UserDialog.Edit(position))
            {
                return;
            }

            try
            {
                RepositoryPositions.Update(position);

                /*убрать*/
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
            return true && SelectedPosition != null && (Positions)p != null;
        }

        private void OnRemovePosition(object p)
        {
            Positions position_to_remove = SelectedPosition ?? (Positions)p;

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
        /// <summary>
        /// Добавление нового раздела
        /// </summary>
        public ICommand AddNewSection { get; }

        private bool CanAddNewSection(object p) => true;

        private void OnAddNewSection(object p)
        {
            Sections new_section = new Sections();

            if (!UserDialog.Edit(new_section))
            {
                return;
            }

            try
            {
                Sections.Add(RepositorySections.Add(new_section));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedSection = new_section;
            }
        }
        /// <summary>
        /// Редактирование раздела
        /// </summary>
        public ICommand EditSection { get; }

        private bool CanEditSection(object p)
        {
            return true && SelectedSection != null && (Sections)p != null;
        }

        private void OnEditSection(object p)
        {
            Sections section = SelectedSection ?? (Sections)p;

            if (!UserDialog.Edit(section))
            {
                return;
            }

            try
            {
                RepositorySections.Update(section);

                /*убрать*/
                if (Sections.Remove(section))
                {
                    Sections.Add(section);
                }
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedSection = section;
            }
        }
        /// <summary>
        /// Удаление раздела
        /// </summary>
        public ICommand RemoveSection { get; }

        private bool CanRemoveSection(object p)
        {
            return true && SelectedSection != null && (Sections)p != null;
        }

        private void OnRemoveSection(object p)
        {
            Sections section_to_remove = SelectedSection ?? (Sections)p;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить раздел {section_to_remove.Name}?", "Удаление раздела"))
            {
                return;
            }

            try
            {
                RepositorySections.Remove(section_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                Sections.Remove(section_to_remove);

                if (ReferenceEquals(SelectedSection, section_to_remove))
                {
                    SelectedSection = null;
                }
            }
        }
        #endregion

        #endregion

        #region КОНСТРУКТОРЫ
        public CompanyManagerViewModel(
            IRepository<Employees> repaUser,
            IRepository<Companies> repaCompany,
            IRepository<Positions> repaPosition,
            IRepository<Sections> repaSections,
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

            AddNewSection = new LambdaCommand(OnAddNewSection, CanAddNewSection);
            EditSection = new LambdaCommand(OnEditSection, CanEditSection);
            RemoveSection = new LambdaCommand(OnRemoveSection, CanRemoveSection);
        }
        #endregion
    }
}