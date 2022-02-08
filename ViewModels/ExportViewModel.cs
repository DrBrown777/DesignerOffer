using Designer_Offer.ViewModels.Base;

namespace Designer_Offer.ViewModels
{
    internal class ExportViewModel : ViewModel
    {
        #region СВОЙСТВА
        private string _ProjectName;
        /// <summary>
        /// Название экспортируемого проекта
        /// </summary>
        public string ProjectName
        {
            get => _ProjectName;
            set => Set(ref _ProjectName, value);
        }

        private string _OfferName;
        /// <summary>
        /// Название экспортируемого КП
        /// </summary>
        public string OfferName
        {
            get => _OfferName;
            set => Set(ref _OfferName, value);
        }

        private bool _SummarySheet = true;
        /// <summary>
        /// Выбор, генирировать ли итоговый лист КП
        /// </summary>
        public bool SummarySheet
        {
            get => _SummarySheet;
            set => Set(ref _SummarySheet, value);
        }

        private bool _InternalUse = true;
        /// <summary>
        /// Выбор схемы экспорта (Внутр. использование / Заказчик)
        /// </summary>
        public bool InternalUse
        {
            get => _InternalUse;
            set => Set(ref _InternalUse, value);
        }
        #endregion
    }
}
