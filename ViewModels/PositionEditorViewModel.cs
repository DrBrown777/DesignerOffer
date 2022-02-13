using Designer_Offer.ViewModels.Base;

namespace Designer_Offer.ViewModels
{
    internal class PositionEditorViewModel : ViewModel
    {
        #region СВОЙСТВА
        private string _Name;
        /// <summary>
        /// Название должности
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
        #endregion
    }
}
