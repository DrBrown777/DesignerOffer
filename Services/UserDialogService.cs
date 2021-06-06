using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels;
using Designer_Offer.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;

namespace Designer_Offer.Services
{
    internal class UserDialogService : IUserDialog
    {
        public bool Edit(object item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            switch (item)
            {
                case Client client:
                    return EditClient(client);
                case Build build:
                    return EditBuild(build);
                default:
                    throw new NotSupportedException($"Редактирование обьекта типа {item.GetType().Name} не поддерживается.");
            }
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

        public void ShowError(string Information, string Caption)
        {
            MessageBox.Show(
                Information, Caption,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public void ShowInformation(string Information, string Caption)
        {
            MessageBox.Show(
               Information, Caption,
               MessageBoxButton.OK,
               MessageBoxImage.Information);
        }

        private static bool EditClient(Client client)
        {
            var client_editor_window = App.Host.Services.GetRequiredService<ClientEditorWindow>();
            var client_editor_model = App.Host.Services.GetRequiredService<ClientEditorViewModel>();

            client_editor_model.Name = client.Name;

            client_editor_window.DataContext = client_editor_model;

            if (client_editor_window.ShowDialog() != true) return false;

            client.Name = client_editor_model.Name;

            return true;
        }

        private static bool EditBuild(Build build)
        {
            var build_editor_window = App.Host.Services
                                        .GetRequiredService<BuildEditorWindow>();
            var build_editor_model = App.Host.Services
                                        .GetRequiredService<BuildEditorViewModel>();

            build_editor_model.Name = build.Name;
            build_editor_model.Adress = build.Adress;
            build_editor_model.Clients = App.Host.Services
                                        .GetRequiredService<ProjectManagerViewModel>()
                                        .Clients;

            build_editor_model.Project = build.Project;
            build_editor_model.SelectedClient = build.Client;

            build_editor_window.DataContext = build_editor_model;

            if (build_editor_window.ShowDialog() != true) return false;

            build.Name = build_editor_model.Name;
            build.Adress = build_editor_model.Adress;
            build.Project = build_editor_model.Project;
            //build.Client = build_editor_model.SelectedClient;
            build.Client_Id = build_editor_model.SelectedClient.Id;

            return true;
        }
    }
}
