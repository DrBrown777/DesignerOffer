using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
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
        #endregion

        #region КОНСТРУКТОРЫ
        public ProjectManagerViewModel(IEntity employe, IRepository<Employee> repaUser, IRepository<Company> repaCompany, 
                                                        IRepository<Client> repaClient, IRepository<Build> repaBuild)
        {
            //CurrentUser = repaUser.Get(employe.Id);
            CurrentUser = repaUser.Get(21);

            CurrentCompany = repaCompany.Get((int)CurrentUser.Company_Id);

            Title = CurrentCompany.Name + _title;

            Status = CurrentUser.First_Name + " " + CurrentUser.Last_Name;

            Clients = new ObservableCollection<Client>(repaClient.Items);

            Builds = new ObservableCollection<Build>(repaBuild.Items);
        }
        #endregion
    }
}
