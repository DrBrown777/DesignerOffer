using Designer_Offer.Data;
using Designer_Offer.ViewModels.Base;

namespace Designer_Offer.ViewModels
{
    internal class ClientEditorViewModel : ViewModel
    {
        /// <summary>
        /// Id клиента
        /// </summary>
        public int ClientId { get; }

        private string _Name;
        /// <summary>
        /// Название клиента
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        public ClientEditorViewModel(Client client)
        {
            ClientId = client.Id;
            Name = client.Name;
        }
    }
}
