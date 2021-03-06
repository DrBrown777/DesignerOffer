namespace Designer_Offer.Services.Interfaces
{
    internal interface IUserDialog
    {
        bool Edit(object item);
        bool ConfirmInformation(string Information, string Caption);
        bool ConfirmWarning(string Information, string Caption);
        bool ConfirmError(string Information, string Caption);
        void ShowError(string Information, string Caption);
        void ShowInformation(string Information, string Caption);
    }
}
