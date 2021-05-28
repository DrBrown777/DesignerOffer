using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    class ProjectManagerViewModel : ViewModel
    {
        #region ПОЛЯ
        static readonly string _title = " :: Управление проектами";
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private Employee CurrentUser;
        /// <summary>
        /// Текущая компания
        /// </summary>
        private Company CurrentCompany;

        #region Репозитории 
        /// <summary>
        /// Репозитории пользователей
        /// </summary>
        private readonly IRepository<Employee> RepositoryUsers;
        /// <summary>
        /// Репозиторий компаний
        /// </summary>
        private readonly IRepository<Company> RepositoryCompanies;
        /// <summary>
        /// Репозиторий клиентов
        /// </summary>
        private readonly IRepository<Client> RepositoryClients;
        /// <summary>
        /// Репозиторий обьектов
        /// </summary>
        private readonly IRepository<Build> RepositoryBuilds;
        /// <summary>
        /// Репозиторий разделов
        /// </summary>
        private readonly IRepository<Section> RepositorySections;
        /// <summary>
        /// Репозиторий КП
        /// </summary>
        private readonly IRepository<Offer> RepositoryOffers;
        /// <summary>
        /// Репозиторий проектов
        /// </summary>
        private readonly IRepository<Project> RepositoryProject;
        /// <summary>
        /// Репозиторий систем в КП
        /// </summary>
        private readonly IRepository<Part> RepositoryParts;
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
                    ClientsViewSource.View.Refresh();
            }
        }

        public ICollectionView ClientsView => ClientsViewSource?.View;

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

        private Project _Project;
        /// <summary>
        /// Текущий проект обьекта
        /// </summary>
        public Project Project
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
        #endregion

        #region КОМАНДЫ
        /// <summary>
        /// Загрузка данных из репозиториев
        /// </summary>
        public ICommand LoadDataFromRepositories { get; }

        private bool CanLoadDataFromRepositories(object p) => true;

        private async void OnLoadDataFromRepositories(object p)
        {
            try
            {
                //CurrentUser = await RepositoryUsers.GetAsync(App.Host.Services.GetRequiredService<IEntity>().Id);
                CurrentUser = await RepositoryUsers.GetAsync(21);

                Status = CurrentUser.First_Name + " " + CurrentUser.Last_Name;

                CurrentCompany = await RepositoryCompanies.GetAsync((int)CurrentUser?.Company_Id);

                Title = CurrentCompany?.Name + _title;

                Clients = new ObservableCollection<Client>(await RepositoryClients.Items.ToListAsync());

                Sections = new ObservableCollection<Section>(await RepositorySections.Items.ToListAsync());

                Builds = new ObservableCollection<Build>(await RepositoryBuilds.Items.ToListAsync());

                Employees = new ObservableCollection<Employee>(await RepositoryUsers.Items.ToListAsync());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Выборка обьектов в зависимости от выбранного клиента
        /// </summary>
        public ICommand FilterBuild { get; }

        private bool CanFilterBuild(object p)
        {
            return true && SelectedClient != null;
        }

        private async void OnFilterBuild(object p)
        {
            var items = await RepositoryBuilds.Items.Where(b => b.Client_Id == SelectedClient.Id).ToListAsync();

            UpdateCollection(Builds, items);
        }

        /// <summary>
        /// Выборка КП в зависимости от выбранного обьекта
        /// </summary>
        public ICommand FilterOffer { get; }

        private bool CanFilterOffer(object p)
        {
            if (SelectedBuild == null)
            {
                if (Offers?.Count != 0) Offers?.Clear();

                return false;
            }

            return true;
        }

        private async void OnFilterOffer(object p)
        {
            var items = await RepositoryOffers.Items.Where(o => o.Project_Id == SelectedBuild.Id).ToListAsync();

            if (SelectedBuild.Project != null)
            {
                Project = await RepositoryProject.GetAsync(SelectedBuild.Project.Id);
            }
            else
            {
                Project = null;
            }

            UpdateCollection(Offers, items);
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

        private async void OnFilterPart(object p)
        {
            var items = await RepositoryParts.Items.Where(part => part.Offer.Id == SelectedOffer.Id).ToListAsync();

            UpdateCollection(Parts, items);
        }
        #endregion

        #region МЕТОДЫ
        private void OnClientFilter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Client client) || string.IsNullOrEmpty(ClientFilter)) return;

            if (!client.Name.ToLower().Contains(ClientFilter.ToLower()))
                e.Accepted = false;
        }

        private void UpdateCollection<T>(ObservableCollection<T> collection, List<T> list)
        {
            if (collection == null || list == null) return;

            collection.Clear();

            foreach (var item in list)
            {
                collection.Add(item);
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public ProjectManagerViewModel(IRepository<Employee> repaUser, IRepository<Company> repaCompany,
                                       IRepository<Client> repaClient, IRepository<Section> repaSection,
                                       IRepository<Build> repaBuild, IRepository<Offer> repaOffer,
                                       IRepository<Project> repaProject, IRepository<Part> repaPart)
        {
            RepositoryUsers = repaUser; RepositoryCompanies = repaCompany;
            RepositoryClients = repaClient; RepositoryBuilds = repaBuild;
            RepositorySections = repaSection; RepositoryOffers = repaOffer;
            RepositoryProject = repaProject; RepositoryParts = repaPart;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);
            FilterBuild = new LambdaCommand(OnFilterBuild, CanFilterBuild);
            FilterOffer = new LambdaCommand(OnFilterOffer, CanFilterOffer);
            FilterPart = new LambdaCommand(OnFilterPart, CanFilterPart);

            Offers = new ObservableCollection<Offer>();
            Parts = new ObservableCollection<Part>();
        }
        #endregion
    }
}
