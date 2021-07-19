using Designer_Offer.ViewModels.Base;

namespace Designer_Offer.ViewModels
{
    internal class SectionEditorViewModel : ViewModel
    {
        private string _Name;
        /// <summary>
        /// Название раздела
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
    }
}
