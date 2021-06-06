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
        static readonly string _title = " :: Управление проектами";
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
                    ClientsViewSource?.View.Refresh();
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
        #endregion

        #region КОМАНДЫ

        #region загрузка данных из репозиториев
        /// <summary>
        /// Загрузка данных из репозиториев
        /// </summary>
        public ICommand LoadDataFromRepositories { get; }

        private bool CanLoadDataFromRepositories(object p)
        {
            if (RepositoryUsers == null || RepositoryClients == null)
                return false;
            else if (RepositorySections == null || RepositoryBuilds == null)
                return false;
            else
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

                Sections = new ObservableCollection<Section>(await RepositorySections.Items.ToListAsync());

                Builds = new ObservableCollection<Build>(await RepositoryBuilds.Items.ToListAsync());
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
            var items = SelectedClient.Build
                .Where(b => b.Client_Id == SelectedClient.Id)
                .ToList();

            UpdateCollection(Builds, items);
        }

        /// <summary>
        /// Выборка КП в зависимости от выбранного обьекта
        /// </summary>
        public ICommand FilterOffer { get; }

        private bool CanFilterOffer(object p)
        {
            if (Project?.Count != 0) Project?.Clear();
            
            if (SelectedBuild == null || SelectedBuild.Project == null)
            {
                if (Offers?.Count != 0) Offers?.Clear();

                return false;
            }
            return true;
        }

        private void OnFilterOffer(object p)
        {
            Project?.Add(SelectedBuild.Project);

            var items = SelectedBuild.Project.Offer
                .Where(o => o.Project_Id == SelectedBuild.Id)
                .ToList();

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

        private void OnFilterPart(object p)
        {
            var items = SelectedOffer.Part
                .Where(part => part.Offer_Id == SelectedOffer.Id)
                .ToList();

            UpdateCollection(Parts, items);
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

            if (!UserDialog.Edit(new_client)) return;

            Clients.Add(RepositoryClients.Add(new_client));

            SelectedClient = new_client;
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
            var client_to_edit = (Client)p ?? SelectedClient;

            if (!UserDialog.Edit(client_to_edit)) return;

            RepositoryClients.Update(client_to_edit);

            ClientsViewSource.View.Refresh();

            SelectedClient = client_to_edit;
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
            var client_to_remove = (Client)p ?? SelectedClient;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить клента {client_to_remove.Name}?", "Удаление клиента"))
                return;

            RepositoryClients.Remove(client_to_remove.Id);

            Clients.Remove(client_to_remove);

            if (ReferenceEquals(SelectedClient, client_to_remove))
                SelectedClient = null;
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
            var new_project = new Project
            {
                Employee_Id = CurrentUser.Id,
                Date = DateTime.Today,
            };

            var new_build = new Build()
            {
                Project = new_project,
                //Client = (Client)p ?? SelectedClient,
            };

            if (!UserDialog.Edit(new_build)) return;

            new_build = RepositoryBuilds.Add(new_build);

            Clients.Remove(Clients.SingleOrDefault(c => c.Id == new_build.Client_Id));

            var new_client = RepositoryClients.Get((int)new_build.Client_Id);

            Clients.Add(new_client);

            ClientsViewSource.View.Refresh();

            OnPropertyChanged(nameof(ClientsView));

            SelectedClient = new_client;
            SelectedBuild = new_build;
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
            var build_to_edit = (Build)p ?? SelectedBuild;

            if (!UserDialog.Edit(build_to_edit)) return;

            RepositoryBuilds.Update(build_to_edit);

            SelectedBuild = build_to_edit;
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
            var build_to_remove = (Build)p ?? SelectedBuild;

            var client_id = (int)build_to_remove.Client_Id;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить обьект {build_to_remove.Project.Name}?", "Удаление обьекта"))
                return;

            SelectedClient.Build.Remove(build_to_remove);

            RepositoryClients.Update(SelectedClient);

            RepositoryBuilds.Remove(build_to_remove.Id);

            Clients.Remove(Clients.SingleOrDefault(c => c.Id == client_id));

            var new_client = RepositoryClients.Get(client_id);

            Clients.Add(new_client);

            ClientsViewSource.View.Refresh();

            OnPropertyChanged(nameof(ClientsView));

            SelectedClient = new_client;

            if (ReferenceEquals(SelectedBuild, build_to_remove))
                SelectedBuild = null;
        }

        #endregion

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
        public ProjectManagerViewModel(
            IRepository<Employee> repaUser,
            IRepository<Client> repaClient,
            IRepository<Section> repaSection,
            IRepository<Build> repaBuild,
            IUserDialog userDialog)
        {
            Progress = true;

            RepositoryUsers = repaUser;
            RepositoryClients = repaClient; 
            RepositoryBuilds = repaBuild;
            RepositorySections = repaSection;

            UserDialog = userDialog;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);
            FilterBuild = new LambdaCommand(OnFilterBuild, CanFilterBuild);
            FilterOffer = new LambdaCommand(OnFilterOffer, CanFilterOffer);
            FilterPart = new LambdaCommand(OnFilterPart, CanFilterPart);

            AddClient = new LambdaCommand(OnAddClient, CanAddClient);
            RemoveClient = new LambdaCommand(OnRemoveClient, CanRemoveClient);
            EditClient = new LambdaCommand(OnEditClient, CanEditClient);

            AddBuild = new LambdaCommand(OnAddBuild, CanAddBuild);
            RemoveBuild = new LambdaCommand(OnRemoveBuild, CanRemoveBuild);
            EditBuild = new LambdaCommand(OnEditBuild, CanEditBuild);

            Offers = new ObservableCollection<Offer>();
            Parts = new ObservableCollection<Part>();
            Project = new ObservableCollection<Project>();
        }
        #endregion
    }
}