using Designer_Offer.ViewModels.Base;

namespace Designer_Offer.ViewModels
{
    internal class PositionEditorViewModel : ViewModel
    {
        private string _Name;
        /// <summary>
        /// Название должности
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
    }
}
