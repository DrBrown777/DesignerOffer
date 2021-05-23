using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Designer_Offer.ViewModels
{
    class ProjectManagerViewModel : ViewModel
    {
        #region ПОЛЯ
        static readonly string _title = " :: Управление проектами";
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private readonly Employee CurrentUser;
        /// <summary>
        /// Текущая компания
        /// </summary>
        private readonly Company CurrentCompany;
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
        #endregion

        #region КОНСТРУКТОРЫ
        public ProjectManagerViewModel(IEntity employe, IRepository<Employee> repaUser, IRepository<Company> repaCompany, 
                                                        IRepository<Client> repaClient, IRepository<Section> repaSection)
        {
            CurrentUser = repaUser.Get(employe.Id);
            //CurrentUser = repaUser.Get(21);

            CurrentCompany = repaCompany.Get((int)CurrentUser.Company_Id);

            Title = CurrentCompany.Name + _title;

            Status = CurrentUser.First_Name + " " + CurrentUser.Last_Name;

            Clients = new ObservableCollection<Client>(repaClient.Items);

            Sections = new ObservableCollection<Section>(repaSection.Items);

        }
        #endregion
    }
}
