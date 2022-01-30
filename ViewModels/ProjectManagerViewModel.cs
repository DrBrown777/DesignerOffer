using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Models;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
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
        private Employees CurrentUser;

        /// <summary>
        /// Текущая компания
        /// </summary>
        private Companies CurrentCompany;

        /// <summary>
        /// Сервис диалогов
        /// </summary>
        private readonly IUserDialog UserDialog;
        /// <summary>
        /// Сервис калькуляции цен
        /// </summary>
        private readonly ICalculator CalculatorService;

        #region Репозитории 
        /// <summary>
        /// Репозитории пользователей
        /// </summary>
        private readonly IRepository<Employees> RepositoryUsers;

        /// <summary>
        /// Репозиторий клиентов
        /// </summary>
        private readonly IRepository<Clients> RepositoryClients;

        /// <summary>
        /// Репозиторий обьектов
        /// </summary>
        private readonly IRepository<Builds> RepositoryBuilds;

        /// <summary>
        /// Репозитоторий КП
        /// </summary>
        private readonly IRepository<Offers> RepositoryOffer;

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

        private ObservableCollection<Clients> _Clients;
        /// <summary>
        /// Коллекция клиентов
        /// </summary>
        public ObservableCollection<Clients> Clients
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
                            new SortDescription(nameof(Data.Clients.Name), ListSortDirection.Ascending)
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

        private Clients _SelectedClient;
        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public Clients SelectedClient
        {
            get => _SelectedClient;
            set => Set(ref _SelectedClient, value);
        }

        private ObservableCollection<Builds> _Builds;
        /// <summary>
        /// Колекция обьектов клиентов
        /// </summary>
        public ObservableCollection<Builds> Builds
        {
            get => _Builds;
            set => Set(ref _Builds, value);
        }

        private Builds _SelectedBuild;
        /// <summary>
        /// Выбранный обьект
        /// </summary>
        public Builds SelectedBuild
        {
            get => _SelectedBuild;
            set => Set(ref _SelectedBuild, value);
        }

        private ObservableCollection<Sections> _Sections;
        /// <summary>
        /// Разделы КП
        /// </summary>
        public ObservableCollection<Sections> Sections
        {
            get => _Sections;
            set => Set(ref _Sections, value);
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

        private ObservableCollection<Offers> _Offers;
        /// <summary>
        /// Колекция КП
        /// </summary>
        public ObservableCollection<Offers> Offers
        {
            get => _Offers;
            set => Set(ref _Offers, value);
        }

        private Offers _SelectedOffer;
        /// <summary>
        /// Выбранное КП
        /// </summary>
        public Offers SelectedOffer
        {
            get => _SelectedOffer;
            set
            {
                if (Set(ref _SelectedOffer, value))
                {
                    if (_SelectedOffer != null)
                    {
                        OfferPrice = CalculatorService.CalculateOfferPrice(_SelectedOffer);
                    }
                }
            }
        }

        private OfferPrice _offerPrice;
        /// <summary>
        /// Суммарные стоимости выбранного КП
        /// </summary>
        public OfferPrice OfferPrice
        {
            get => _offerPrice;
            set => Set(ref _offerPrice, value);
        }

        private ObservableCollection<Projects> _Project;
        /// <summary>
        /// Текущий проект обьекта
        /// </summary>
        public ObservableCollection<Projects> Project
        {
            get => _Project;
            set => Set(ref _Project, value);
        }

        private ObservableCollection<PartPrice> _Parts;
        /// <summary>
        /// Коллекция систем
        /// </summary>
        public ObservableCollection<PartPrice> Parts
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
        private Sections _SelectedSection;
        /// <summary>
        /// Выбранный раздел
        /// </summary>
        public Sections SelectedSection
        {
            get => _SelectedSection;
            set => Set(ref _SelectedSection, value);
        }
        private Employees _SelectedManager;
        /// <summary>
        /// Выбранный менеджер
        /// </summary>
        public Employees SelectedManager
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
                Employees = new ObservableCollection<Employees>(await RepositoryUsers.Items.ToListAsync());

                //CurrentUser = Employees.SingleOrDefault(e => e.Id == App.Host.Services.GetRequiredService<Employee>().Id);

                CurrentUser = Employees.SingleOrDefault(e => e.Id == 21);

                Status = CurrentUser.First_Name + " " + CurrentUser.Last_Name;

                CurrentCompany = CurrentUser.Companies;

                Title = CurrentCompany?.Name + _title;

                Clients = new ObservableCollection<Clients>(await RepositoryClients.Items.ToListAsync());

                FilterBuild.Execute(null);

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
            List<Builds> items = SelectedClient.Builds
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

            if (SelectedBuild?.Projects.Offers.Count == 0 || SelectedClient.Builds?.Count == 0)
            {
                OfferPrice = null;
            }

            if (SelectedBuild == null || SelectedBuild.Projects == null)
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
            Project?.Add(SelectedBuild.Projects);

            List<Offers> items = SelectedBuild.Projects.Offers
                .Where(o => o.Project_Id == SelectedBuild.Id)
                .ToList();

            UpdateCollection(Offers, items);

            if (SelectedBuild.Projects.Offers != null)
            {
                if (SelectedBuild.Projects.Offers.Count != 0)
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
            List<PartPrice> items = CalculatorService.CalculatePartPrice(SelectedOffer.Parts);

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
            List<Offers> items = SelectedBuild.Projects.Offers
                .Where(o => o.Section_Id == SelectedSection.Id)
                .Where(o => o.Projects.Id == SelectedBuild.Id)
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
            List<Builds> items = SelectedClient.Builds
                .Where(b => b.Projects.Employee_Id == SelectedManager.Id)
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
            List<Offers> items = SelectedBuild.Projects.Offers
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
            var new_client = new Clients();

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
            return (Clients)p != null && SelectedClient != null;
        }

        private void OnEditClient(object p)
        {
            Clients client_to_edit = (Clients)p ?? SelectedClient;

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
            return (Clients)p != null && SelectedClient != null;
        }

        private void OnRemoveClient(object p)
        {
            Clients client_to_remove = (Clients)p ?? SelectedClient;

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
            return (Clients)p != null && SelectedClient != null;
        }

        private void OnAddBuild(object p)
        {
            Projects new_project = new Projects
            {
                Employee_Id = CurrentUser.Id,
                Date = DateTime.Today,
            };

            Builds new_build = new Builds()
            {
                Projects = new_project,
                Client_Id = SelectedClient.Id,
            };

            if (!UserDialog.Edit(new_build))
            {
                return;
            }

            try
            {
                SelectedClient.Builds.Add(new_build);

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
            return (Builds)p != null && SelectedBuild != null && SelectedClient != null;
        }

        private void OnEditBuild (object p)
        {
            Builds build_to_edit = (Builds)p ?? SelectedBuild;

            if (!UserDialog.Edit(build_to_edit))
            {
                return;
            }

            try
            {
                RepositoryClients.Update(build_to_edit.Clients);
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

                SelectedClient = build_to_edit.Clients;
                SelectedBuild = build_to_edit;
            }
        }
        /// <summary>
        /// Удаление обьекта
        /// </summary>
        public ICommand RemoveBuild { get; }

        private bool CanRemoveBuild (object p)
        {
            return (Builds)p != null && SelectedBuild != null && SelectedClient != null;
        }

        private void OnRemoveBuild (object p)
        {
            Builds build_to_remove = (Builds)p ?? SelectedBuild;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить обьект {build_to_remove.Projects.Name}?", "Удаление обьекта"))
            {
                return;
            }

            try
            {
                SelectedClient.Builds.Remove(build_to_remove);

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
            return (Builds)p != null && SelectedBuild != null;
        }

        private void OnAddOffer(object p)
        {
            Configs config = new Configs()
            {
                Margin_Product = _marginProduct,
                Margin_Work = _marginInstall,
                Margin_Admin = _marginAdmin
            };
            Offers new_offer = new Offers()
            {
                Configs = config,
                Projects = SelectedBuild.Projects,
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

                OnPropertyChanged(nameof(SelectedBuild.Projects.Offers));

                SelectedOffer = new_offer;
            }
        }
        /// <summary>
        /// Редактирование КП
        /// </summary>
        public ICommand EditOffer { get; }
        
        private bool CanEditOffer(object p)
        {
            return (Offers)p != null && SelectedBuild != null && SelectedOffer != null;
        }

        private void OnEditOffer(object p)
        {
            Offers offer_to_edit = (Offers)p ?? SelectedOffer;

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

                OnPropertyChanged(nameof(SelectedBuild.Projects.Offers));

                SelectedOffer = offer_to_edit;
            }
        }
        /// <summary>
        /// Удаление КП
        /// </summary>
        public ICommand RemoveOffer { get; }

        private bool CanRemoveOffer(object p)
        {
            return (Offers)p != null && SelectedBuild != null && SelectedOffer != null;
        }

        private void OnRemoveOffer(object p)
        {
            Offers offer_to_remove = (Offers)p ?? SelectedOffer;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить {offer_to_remove.Name}?", "Удаление КП"))
            {
                return;
            }

            try
            {
                RepositoryOffer.Remove(offer_to_remove.Id);

                SelectedBuild.Projects.Offers.Remove(offer_to_remove);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                FilterBuild.Execute(null);

                OnPropertyChanged(nameof(SelectedBuild.Projects.Offers));

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
            if (!(e.Item is Clients client) || string.IsNullOrEmpty(ClientFilter))
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

            foreach (T item in list)
            {
                collection.Add(item);
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public ProjectManagerViewModel(
            IRepository<Employees> repaUser,
            IRepository<Clients> repaClient,
            IRepository<Sections> repaSection,
            IRepository<Builds> repaBuild,
            IRepository<Offers> repaOffer,
            IUserDialog userDialog, ICalculator calcService)
        {
            Progress = true;

            RepositoryUsers = repaUser;
            RepositoryClients = repaClient; 
            RepositoryBuilds = repaBuild;
            RepositoryOffer = repaOffer;
            RepositorySections = repaSection;

            UserDialog = userDialog;
            CalculatorService = calcService;

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

            Builds = new ObservableCollection<Builds>();
            Offers = new ObservableCollection<Offers>();
            Parts = new ObservableCollection<PartPrice>();
            Project = new ObservableCollection<Projects>();

            StartSelectedDate = DateTime.Now;
            EndSelectedDate = DateTime.Now;
        }
        #endregion
    }
}