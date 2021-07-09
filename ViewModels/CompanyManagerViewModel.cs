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
            Company new_company = new Company
            {
                Position = Positions
            };

            if (!UserDialog.Edit(new_company))
            {
                return;
            }

            try
            {
                foreach (Position item in Positions)
                {
                    if (new_company.Position.Contains(item))
                    {
                        item.Company.Add(new_company);
                        RepositoryPositions.Update(item);
                    }
                }

                Companies.Add(new_company);
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
        }
        #endregion
    }
}