using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels;
using Designer_Offer.Views.Windows;
using System.Windows;

namespace Designer_Offer.Services
{
    internal class UserDialogService : IUserDialog
    {
        public bool Edit(Client client)
        {
            var client_editor_model = new ClientEditorViewModel(client);

            var client_editor_window = new ClientEditorWindow
            {
                DataContext = client_editor_model
            };

            if (client_editor_window.ShowDialog() != true) return false;

            client.Name = client_editor_model.Name;

            return true;
        }

        public bool ConfirmInformation(string Information, string Caption)
        {
            return MessageBox.Show(
                Information, Caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Information) == MessageBoxResult.Yes;
        }

        public bool ConfirmWarning(string Information, string Caption)
        {
            return MessageBox.Show(
                Information, Caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        public bool ConfirmError(string Information, string Caption)
        {
            return MessageBox.Show(
                Information, Caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Error) == MessageBoxResult.Yes;
        }
    }
}
