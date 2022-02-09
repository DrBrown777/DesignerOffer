using Designer_Offer.Data;
using System.Collections.Generic;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal interface IRegistrationService
    {
        List<Companies> Companies { get; set; }

        ICommand LoadLoginPageCommand { get; set; }

        void Update(List<Companies> companies);
    }
}