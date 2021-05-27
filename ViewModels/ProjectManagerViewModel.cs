using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System;
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

        private ObservableCollection<Build> _Builds;
        /// <summary>
        /// Колекция обьектов клиентов
        /// </summary>
        public ObservableCollection<Build> Builds
        {
            get => _Builds;
            set => Set(ref _Builds, value);
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

        private Client _SelectedClient;
        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public Client SelectedClient
        {
            get => _SelectedClient;
            set => Set(ref _SelectedClient, value);
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

        private Offer _SelectedOffer;
        /// <summary>
        /// Выбранное КП
        /// </summary>
        public Offer SelectedOffer
        {
            get => _SelectedOffer;
            set => Set(ref _SelectedOffer, value);
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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Фильтрация Обьектов в зависимости от выбранного клиента
        /// </summary>
        public ICommand FilterBuild { get; }

        private bool CanFilterBuild(object p)
        {
            return true && SelectedClient != null;
        }

        private async void OnFilterBuild(object p)
        {
            var items = await RepositoryBuilds.Items.Where(b => b.Client_Id == SelectedClient.Id).ToListAsync();

            if (Builds != null)
            {
                Builds.Clear();

                foreach (var item in items)
                {
                    Builds.Add(item);
                }
            }
        }
        #endregion

        #region МЕТОДЫ
        private void OnClientFilter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Client client) || string.IsNullOrEmpty(ClientFilter)) return;

            if (!client.Name.ToLower().Contains(ClientFilter.ToLower()))
                e.Accepted = false;
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public ProjectManagerViewModel(IRepository<Employee> repaUser, IRepository<Company> repaCompany,
                                       IRepository<Client> repaClient, IRepository<Section> repaSection,
                                       IRepository<Build> repaBuild)
        {
            RepositoryUsers = repaUser;
            RepositoryCompanies = repaCompany;
            RepositoryClients = repaClient;
            RepositoryBuilds = repaBuild;
            RepositorySections = repaSection;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);
            FilterBuild = new LambdaCommand(OnFilterBuild, CanFilterBuild);
        }
        #endregion
    }
}
