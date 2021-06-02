using Designer_Offer.ViewModels.Base;

namespace Designer_Offer.ViewModels
{
    internal class ClientEditorViewModel : ViewModel
    {
        private string _Name;
        /// <summary>
        /// Название клиента
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
    }
}
