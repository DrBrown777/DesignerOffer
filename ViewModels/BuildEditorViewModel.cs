using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Designer_Offer.ViewModels
{
    internal class BuildEditorViewModel : ViewModel
    {
        private string _Name;
        /// <summary>
        /// Полное название стройки
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private string _Adress;
        /// <summary>
        /// Адресс стройки
        /// </summary>
        public string Adress
        {
            get => _Adress;
            set => Set(ref _Adress, value);
        }

        private ObservableCollection<Client> _Clients;
        /// <summary>
        /// Список Клиентов
        /// </summary>
        public ObservableCollection<Client> Clients
        {
            get => _Clients;
            set => Set(ref _Clients, value);
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

        private Project _Project;
        /// <summary>
        /// Проект для обьекта
        /// </summary>
        public Project Project
        {
            get => _Project;
            set => Set(ref _Project, value);
        }
    }
}
