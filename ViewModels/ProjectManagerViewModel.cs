using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;
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
            set => Set(ref _Clients, value);
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

        private ObservableCollection<Section> _Sections;
        /// <summary>
        /// Разделы КП
        /// </summary>
        public  ObservableCollection<Section> Sections
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
            //CurrentUser = await RepositoryUsers.GetAsync(App.Host.Services.GetRequiredService<IEntity>().Id);
            CurrentUser = await RepositoryUsers.GetAsync(21);

            Status = CurrentUser.First_Name + " " + CurrentUser.Last_Name;

            CurrentCompany = await RepositoryCompanies.GetAsync((int)CurrentUser?.Company_Id);

            Title = CurrentCompany?.Name + _title;

            Clients = new ObservableCollection<Client>(await RepositoryClients.Items.ToListAsync());

            Sections = new ObservableCollection<Section>(await RepositorySections.Items.ToListAsync());
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public ProjectManagerViewModel(IRepository<Employee> repaUser, IRepository<Company> repaCompany, 
                                       IRepository<Client> repaClient, IRepository<Section> repaSection)
        {
            RepositoryUsers = repaUser;
            RepositoryCompanies = repaCompany;
            RepositoryClients = repaClient;
            RepositorySections = repaSection;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);
        }
        #endregion
    }
}
