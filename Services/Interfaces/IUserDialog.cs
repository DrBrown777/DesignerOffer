using Designer_Offer.Data;

namespace Designer_Offer.Services.Interfaces
{
    internal interface IUserDialog
    {
        bool Edit(Client client);

        bool ConfirmInformation(string Information, string Caption);
        bool ConfirmWarning(string Information, string Caption);
        bool ConfirmError(string Information, string Caption);
        void ShowError(string Information, string Caption);
        void ShowInformation(string Information, string Caption);
    }
}
