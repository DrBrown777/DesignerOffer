using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System.Collections.ObjectModel;

namespace Designer_Offer.ViewModels
{
    class ProjectManagerViewModel : ViewModel
    {
        #region ПОЛЯ
        private readonly Employee CurrentUser;

        private readonly Company CurrentCompany;
        #endregion

        #region СВОЙСТВА
        private ObservableCollection<Client> _Clients;

        public ObservableCollection<Client> Clients
        {
            get => _Clients;
            set => Set(ref _Clients, value);
        }

        private ObservableCollection<Build> _Builds;

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
            CurrentUser = repaUser.Get(22);

            CurrentCompany = repaCompany.Get((int)CurrentUser.Company_Id);

            Clients = new ObservableCollection<Client>(repaClient.Items);

            Builds = new ObservableCollection<Build>(repaBuild.Items);
        }
        #endregion
    }
}
