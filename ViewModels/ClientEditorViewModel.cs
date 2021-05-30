using Designer_Offer.ViewModels.Base;

namespace Designer_Offer.ViewModels
{
    internal class ClientEditorViewModel : ViewModel
    {
        /// <summary>
        /// Id клиента
        /// </summary>
        public int Id { get; set; }

        private string _Name;
        /// <summary>
        /// Название клиента
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        public ClientEditorViewModel() { }
    }
}
