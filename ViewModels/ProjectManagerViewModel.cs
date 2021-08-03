using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class ProjectManagerViewModel : ViewModel
    {
        #region ПОЛЯ
        private static readonly string _title = " :: Управление проектами";

        /// <summary>
        /// Коэфициент наценки на материал
        /// </summary>
        private const decimal _marginProduct = 1.20M;

        /// <summary>
        /// Коэфициент наценки на работы
        /// </summary>
        private const decimal _marginInstall = 1.40M;

        /// <summary>
        /// Коэфициент административных расходов
        /// </summary>
        private const decimal _marginAdmin = 0.05M;

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

        #region Репозитории 
        /// <summary>
        /// Репозитории пользователей
        /// </summary>
        private readonly IRepository<Employee> RepositoryUsers;

        /// <summary>
        /// Репозиторий клиентов
        /// </summary>
        private readonly IRepository<Client> RepositoryClients;

        /// <summary>
        /// Репозиторий обьектов
        /// </summary>
        private readonly IRepository<Build> RepositoryBuilds;

        /// <summary>
        /// Репозитоторий КП
        /// </summary>
        private readonly IRepository<Offer> RepositoryOffer;

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

        private ObservableCollection<Client> _Clients;
        /// <summary>
        /// Коллекция клиентов
        /// </summary>
        public ObservableCollection<Client> Clients
        {
            get => _Clients;
            set
            {
                if (Set(ref _Clients, value))
                {
                    ClientsViewSource = new CollectionViewSource()
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Client.Name), ListSortDirection.Ascending)
                        }

                    };
                    ClientsViewSource.Filter += OnClientFilter;
                    ClientsViewSource.View.Refresh();

                    OnPropertyChanged(nameof(ClientsView));
                }
            }
        }

        private string _ClientFilter;
        /// <summary>
        /// Искомый клиент для фильтрации
        /// </summary>
        public string ClientFilter
        {
            get => _ClientFilter;
            set
            {
                if (Set(ref _ClientFilter, value))
                {
                    ClientsViewSource?.View.Refresh();
                }
            }
        }
        /// <summary>
        /// Пользовательская сортировка клиентов
        /// </summary>
        public ICollectionView ClientsView => ClientsViewSource?.View;
        /// <summary>
        /// Прокси коллекция клиентов
        /// </summary>
        private CollectionViewSource ClientsViewSource;

        private Client _SelectedClient;
        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public Client SelectedClient
        {
            get => _SelectedClient;
            set => Set(ref _SelectedClient, value);
        }

        private ObservableCollection<Build> _Builds;
        /// <summary>
        /// Колекция обьектов клиентов
        /// </summary>
        public ObservableCollection<Build> Builds
        {
            get => _Builds;
            set => Set(ref _Builds, value);
        }

        private Build _SelectedBuild;
        /// <summary>
        /// Выбранный обьект
        /// </summary>
        public Build SelectedBuild
        {
            get => _SelectedBuild;
            set => Set(ref _SelectedBuild, value);
        }

        private ObservableCollection<Section> _Sections;
        /// <summary>
        /// Разделы КП
        /// </summary>
        public ObservableCollection<Section> Sections
        {
            get => _Sections;
            set => Set(ref _Sections, value);
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

        private ObservableCollection<Offer> _Offers;
        /// <summary>
        /// Колекция КП
        /// </summary>
        public ObservableCollection<Offer> Offers
        {
            get => _Offers;
            set => Set(ref _Offers, value);
        }

        private Offer _SelectedOffer;
        /// <summary>
        /// Выбранное КП
        /// </summary>
        public Offer SelectedOffer
        {
            get => _SelectedOffer;
            set => Set(ref _SelectedOffer, value);
        }

        private ObservableCollection<Project> _Project;
        /// <summary>
        /// Текущий проект обьекта
        /// </summary>
        public ObservableCollection<Project> Project
        {
            get => _Project;
            set => Set(ref _Project, value);
        }

        private ObservableCollection<Part> _Parts;
        /// <summary>
        /// Коллекция систем
        /// </summary>
        public ObservableCollection<Part> Parts
        {
            get => _Parts;
            set => Set(ref _Parts, value);
        }
        private DateTime _StartSelectedDate;
        /// <summary>
        /// Начальная дата
        /// </summary>
        public DateTime StartSelectedDate
        {
            get => _StartSelectedDate;
            set => Set(ref _StartSelectedDate, value);
        }
        private DateTime _EndSelectedDate;
        /// <summary>
        /// Конечная дата
        /// </summary>
        public DateTime EndSelectedDate
        {
            get => _EndSelectedDate;
            set => Set(ref _EndSelectedDate, value);
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
        private Employee _SelectedManager;
        /// <summary>
        /// Выбранный менеджер
        /// </summary>
        public Employee SelectedManager
        {
            get => _SelectedManager;
            set => Set(ref _SelectedManager, value);
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
            if (RepositoryUsers == null || RepositoryClients == null || RepositorySections == null)
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

                Clients = new ObservableCollection<Client>(await RepositoryClients.Items.ToListAsync());

                FilterBuild.Execute(null);

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

        #region фильтрация данных
        /// <summary>
        /// Выборка обьектов в зависимости от выбранного клиента
        /// </summary>
        public ICommand FilterBuild { get; }

        private bool CanFilterBuild(object p)
        {
            return true && SelectedClient != null;
        }

        private void OnFilterBuild(object p)
        {
            List<Build> items = SelectedClient.Build
                .Where(b => b.Client_Id == SelectedClient.Id)
                .ToList();

            UpdateCollection(Builds, items);
            
            if (Builds != null)
            {
                if (Builds.Count != 0)
                {
                    SelectedBuild = Builds[0];
                }
            }
        }

        /// <summary>
        /// Выборка КП в зависимости от выбранного обьекта
        /// </summary>
        public ICommand FilterOffer { get; }

        private bool CanFilterOffer(object p)
        {
            if (Project?.Count != 0)
            {
                Project?.Clear();
            }

            if (SelectedBuild == null || SelectedBuild.Project == null)
            {
                if (Offers?.Count != 0)
                {
                    Offers?.Clear();
                }

                return false;
            }
            return true;
        }

        private void OnFilterOffer(object p)
        {
            Project?.Add(SelectedBuild.Project);

            List<Offer> items = SelectedBuild.Project.Offer
                .Where(o => o.Project_Id == SelectedBuild.Id)
                .ToList();

            UpdateCollection(Offers, items);

            if (SelectedBuild.Project.Offer != null)
            {
                if (SelectedBuild.Project.Offer.Count != 0)
                {
                    SelectedOffer = Offers[0];
                }
            }
        }

        /// <summary>
        /// Выборка систем в зависимости от выбранного КП
        /// </summary>
        public ICommand FilterPart { get; }

        private bool CanFilterPart(object p)
        {
            if (SelectedOffer == null)
            {
                if (Parts?.Count != 0) Parts.Clear();

                return false;
            }

            return true;
        }

        private void OnFilterPart(object p)
        {
            List<Part> items = SelectedOffer.Part
                .Where(part => part.Offer_Id == SelectedOffer.Id)
                .ToList();

            UpdateCollection(Parts, items);
        }
        /// <summary>
        /// Выборка КП по разделу
        /// </summary>
        public ICommand FilterSection { get; }

        private bool CanFilterSection(object p)
        {
            return true && SelectedSection != null && SelectedBuild != null;
        }

        private void OnFilterSection(object p)
        {
            List<Offer> items = SelectedBuild.Project.Offer
                .Where(o => o.Section_Id == SelectedSection.Id)
                .Where(o => o.Project.Id == SelectedBuild.Id)
                .ToList();
            
            UpdateCollection(Offers, items);

            if (Offers.Count != 0)
            {
                SelectedOffer = Offers[0];
            }
        }
        /// <summary>
        /// Выборка строек по менеджеру
        /// </summary>
        public ICommand FilterManager { get; }

        private bool CanFilterManager(object p)
        {
            return true && SelectedManager != null && SelectedClient != null;
        }

        private void OnFilterManager(object p)
        {
            List<Build> items = SelectedClient.Build
                .Where(b => b.Project.Employee_Id == SelectedManager.Id)
                .Where(b => b.Client_Id == SelectedClient.Id)
                .ToList();

            UpdateCollection(Builds, items);

            if (Builds.Count != 0)
            {
                SelectedBuild = Builds[0];
            }
        }
        /// <summary>
        /// Выборка по дате
        /// </summary>
        public ICommand FilterDateDiff { get; }

        public bool CanFilterDateDiff(object p)
        {
            return true && StartSelectedDate < EndSelectedDate
                        && SelectedBuild != null;
        }

        public void OnFilterDateDiff(object p)
        {
            List<Offer> items = SelectedBuild.Project.Offer
                .Where(o => o.Date <= EndSelectedDate && o.Date >= StartSelectedDate)
                .ToList();

            UpdateCollection(Offers, items);

            if (Offers.Count != 0)
            {
                SelectedOffer = Offers[0];
            }
        }
        #endregion

        #region добавление/удаление данных
        /// <summary>
        /// Добавление нового клиента
        /// </summary>
        public ICommand AddClient { get; }

        private bool CanAddClient(object p) => true;

        private void OnAddClient(object p)
        {
            var new_client = new Client();

            if (!UserDialog.Edit(new_client))
            {
                return;
            }

            try
            {
                Clients.Add(RepositoryClients.Add(new_client));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedClient = new_client;
            }
        }
        /// <summary>
        /// Редактирование клиента
        /// </summary>
        public ICommand EditClient { get; }

        private bool CanEditClient(object p)
        {
            return (Client)p != null && SelectedClient != null;
        }

        private void OnEditClient(object p)
        {
            Client client_to_edit = (Client)p ?? SelectedClient;

            if (!UserDialog.Edit(client_to_edit))
            {
                return;
            }

            try
            {
                RepositoryClients.Update(client_to_edit);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                ClientsViewSource.View.Refresh();

                SelectedClient = client_to_edit;
            }
        }
        /// <summary>
        /// Удаление клиента
        /// </summary>
        public ICommand RemoveClient { get; }

        private bool CanRemoveClient(object p)
        {
            return (Client)p != null && SelectedClient != null;
        }

        private void OnRemoveClient(object p)
        {
            Client client_to_remove = (Client)p ?? SelectedClient;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить клента {client_to_remove.Name}?", "Удаление клиента"))
            {
                return;
            }

            try
            {
                RepositoryClients.Remove(client_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                Clients.Remove(client_to_remove);

                if (ReferenceEquals(SelectedClient, client_to_remove))
                {
                    SelectedClient = null;
                }
            }
        }

        /// <summary>
        /// Добаление нового обьекта
        /// </summary>
        public ICommand AddBuild { get; }

        private bool CanAddBuild(object p)
        {
            return (Client)p != null && SelectedClient != null;
        }

        private void OnAddBuild(object p)
        {
            Project new_project = new Project
            {
                Employee_Id = CurrentUser.Id,
                Date = DateTime.Today,
            };

            Build new_build = new Build()
            {
                Project = new_project,
                Client_Id = SelectedClient.Id,
            };

            if (!UserDialog.Edit(new_build))
            {
                return;
            }

            try
            {
                SelectedClient.Build.Add(new_build);

                RepositoryClients.Update(SelectedClient);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                FilterBuild.Execute(null);

                ClientsViewSource.View.Refresh();

                OnPropertyChanged(nameof(ClientsView));

                SelectedBuild = new_build;
            }
        }
        /// <summary>
        /// Редактирование обьекта
        /// </summary>
        public ICommand EditBuild { get; }

        private bool CanEditBuild (object p)
        {
            return (Build)p != null && SelectedBuild != null && SelectedClient != null;
        }

        private void OnEditBuild (object p)
        {
            Build build_to_edit = (Build)p ?? SelectedBuild;

            if (!UserDialog.Edit(build_to_edit))
            {
                return;
            }

            try
            {
                RepositoryClients.Update(build_to_edit.Client);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                FilterBuild.Execute(null);

                ClientsViewSource.View.Refresh();

                OnPropertyChanged(nameof(ClientsView));

                SelectedClient = build_to_edit.Client;
                SelectedBuild = build_to_edit;
            }
        }
        /// <summary>
        /// Удаление обьекта
        /// </summary>
        public ICommand RemoveBuild { get; }

        private bool CanRemoveBuild (object p)
        {
            return (Build)p != null && SelectedBuild != null && SelectedClient != null;
        }

        private void OnRemoveBuild (object p)
        {
            Build build_to_remove = (Build)p ?? SelectedBuild;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить обьект {build_to_remove.Project.Name}?", "Удаление обьекта"))
            {
                return;
            }

            try
            {
                SelectedClient.Build.Remove(build_to_remove);

                RepositoryClients.Update(SelectedClient);

                RepositoryBuilds.Remove(build_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                FilterBuild.Execute(null);

                ClientsViewSource.View.Refresh();

                OnPropertyChanged(nameof(ClientsView));

                if (ReferenceEquals(SelectedBuild, build_to_remove))
                {
                    SelectedBuild = null;
                }
            }
        }
        /// <summary>
        /// Добавление нового КП
        /// </summary>
        public ICommand AddOffer { get; }

        private bool CanAddOffer(object p)
        {
            return (Build)p != null && SelectedBuild != null;
        }

        private void OnAddOffer(object p)
        {
            Config config = new Config()
            {
                Margin_Product = _marginProduct,
                Margin_Work = _marginInstall,
                Margin_Admin = _marginAdmin
            };
            Offer new_offer = new Offer()
            {
                Config = config,
                Project = SelectedBuild.Project,
                Date = DateTime.Today
            };

            if (!UserDialog.Edit(new_offer))
            {
                return;
            }

            try
            {
                Offers.Add(RepositoryOffer.Add(new_offer));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                FilterBuild.Execute(null);

                OnPropertyChanged(nameof(SelectedBuild.Project.Offer));

                SelectedOffer = new_offer;
            }
        }
        /// <summary>
        /// Редактирование КП
        /// </summary>
        public ICommand EditOffer { get; }
        
        private bool CanEditOffer(object p)
        {
            return (Offer)p != null && SelectedBuild != null && SelectedOffer != null;
        }

        private void OnEditOffer(object p)
        {
            Offer offer_to_edit = (Offer)p ?? SelectedOffer;

            if (!UserDialog.Edit(offer_to_edit))
            {
                return;
            }

            try
            {
                RepositoryOffer.Update(offer_to_edit);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                offer_to_edit.Date = DateTime.Today;

                FilterBuild.Execute(null);

                OnPropertyChanged(nameof(SelectedBuild.Project.Offer));

                SelectedOffer = offer_to_edit;
            }
        }
        /// <summary>
        /// Удаление КП
        /// </summary>
        public ICommand RemoveOffer { get; }

        private bool CanRemoveOffer(object p)
        {
            return (Offer)p != null && SelectedBuild != null && SelectedOffer != null;
        }

        private void OnRemoveOffer(object p)
        {
            Offer offer_to_remove = (Offer)p ?? SelectedOffer;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить {offer_to_remove.Name}?", "Удаление КП"))
            {
                return;
            }

            try
            {
                RepositoryOffer.Remove(offer_to_remove.Id);

                SelectedBuild.Project.Offer.Remove(offer_to_remove);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                FilterBuild.Execute(null);

                OnPropertyChanged(nameof(SelectedBuild.Project.Offer));

                if (ReferenceEquals(SelectedOffer, offer_to_remove))
                {
                    SelectedOffer = null;
                }
            }
        }
        #endregion

        #endregion

        #region МЕТОДЫ
        private void OnClientFilter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Client client) || string.IsNullOrEmpty(ClientFilter))
            {
                return;
            }

            if (!client.Name.ToLower().Contains(ClientFilter.ToLower()))
            {
                e.Accepted = false;
            }
        }

        private void UpdateCollection<T>(ObservableCollection<T> collection, List<T> list)
        {
            if (collection == null || list == null)
            {
                return;
            }

            collection.Clear();

            foreach (var item in list)
            {
                collection.Add(item);
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public ProjectManagerViewModel(
            IRepository<Employee> repaUser,
            IRepository<Client> repaClient,
            IRepository<Section> repaSection,
            IRepository<Build> repaBuild,
            IRepository<Offer> repaOffer,
            IUserDialog userDialog)
        {
            Progress = true;

            RepositoryUsers = repaUser;
            RepositoryClients = repaClient; 
            RepositoryBuilds = repaBuild;
            RepositoryOffer = repaOffer;
            RepositorySections = repaSection;

            UserDialog = userDialog;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);
            FilterBuild = new LambdaCommand(OnFilterBuild, CanFilterBuild);
            FilterOffer = new LambdaCommand(OnFilterOffer, CanFilterOffer);
            FilterPart = new LambdaCommand(OnFilterPart, CanFilterPart);
            FilterSection = new LambdaCommand(OnFilterSection, CanFilterSection);
            FilterManager = new LambdaCommand(OnFilterManager, CanFilterManager);
            FilterDateDiff = new LambdaCommand(OnFilterDateDiff, CanFilterDateDiff);

            AddClient = new LambdaCommand(OnAddClient, CanAddClient);
            RemoveClient = new LambdaCommand(OnRemoveClient, CanRemoveClient);
            EditClient = new LambdaCommand(OnEditClient, CanEditClient);

            AddBuild = new LambdaCommand(OnAddBuild, CanAddBuild);
            RemoveBuild = new LambdaCommand(OnRemoveBuild, CanRemoveBuild);
            EditBuild = new LambdaCommand(OnEditBuild, CanEditBuild);

            AddOffer = new LambdaCommand(OnAddOffer, CanAddOffer);
            EditOffer = new LambdaCommand(OnEditOffer, CanEditOffer);
            RemoveOffer = new LambdaCommand(OnRemoveOffer, CanRemoveOffer);

            Builds = new ObservableCollection<Build>();
            Offers = new ObservableCollection<Offer>();
            Parts = new ObservableCollection<Part>();
            Project = new ObservableCollection<Project>();

            StartSelectedDate = DateTime.Now;
            EndSelectedDate = DateTime.Now;
        }
        #endregion
    }
}