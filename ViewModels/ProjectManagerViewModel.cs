using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;

namespace Designer_Offer.ViewModels
{
    class ProjectManagerViewModel : ViewModel
    {
        private readonly Employee CurrentUser;
        private readonly Company CurrentCompany;
        #region СВОЙСТВА


        #endregion
        public ProjectManagerViewModel(IEntity employe, IRepository<Employee> repaUser, IRepository<Company> repaCompany)
        {
            CurrentUser = repaUser.Get(employe.Id);

            CurrentCompany = repaCompany.Get((int)CurrentUser.Company_Id);
        }
    }
}
