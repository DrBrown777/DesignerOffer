using Designer_Offer.ViewModels.Base;

namespace Designer_Offer.ViewModels
{
    class ProjectManagerViewModel : ViewModel
    {
        #region СВОЙСТВА

        private string _Title = "Управление проектами";
        /// <summary>
        /// Заголовок Окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private string _Status;
        /// <summary>
        /// Статус программы
        /// </summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }
        #endregion
    }
}
