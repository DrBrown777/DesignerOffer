using Designer_Offer.ViewModels.Base;

namespace Designer_Offer.ViewModels
{
    internal class UnitEditorViewModel : ViewModel
    {
        private string _Name;
        /// <summary>
        /// Наименование ед.измерения
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
    }
}
