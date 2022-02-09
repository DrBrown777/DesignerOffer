using Designer_Offer.Data;
using System.Collections.Generic;
using System.Windows.Input;

namespace Designer_Offer.Services
{
    internal interface ILoginService
    {
        List<Companies> Companies { get; set; }

        ICommand LoadRegistarationPageCommand { get; set; }

        void Update(List<Companies> companies);
    }
}