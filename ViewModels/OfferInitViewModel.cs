using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace Designer_Offer.ViewModels
{
    internal class OfferInitViewModel : ViewModel
    {
        #region СВОЙСТВА
        /// <summary>
        /// Краткое название обьекта
        /// </summary>
        public string NameProject { get; set; }

        private string _Name;
        /// <summary>
        /// Название КП
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private decimal _MarginProduct;
        /// <summary>
        /// Наценка на материалы
        /// </summary>
        public decimal MarginProduct
        {
            get => _MarginProduct;
            set => Set(ref _MarginProduct, value);
        }

        private decimal _MarginInstall;
        /// <summary>
        /// Наценка на работы
        /// </summary>
        public decimal MarginInstall
        {
            get => _MarginInstall;
            set => Set(ref _MarginInstall, value);
        }

        private decimal _MarginAdmin;
        /// <summary>
        /// Админ расходы
        /// </summary>
        public decimal MarginAdmin
        {
            get => _MarginAdmin;
            set => Set(ref _MarginAdmin, value);
        }

        private List<Sections> _Sections;
        /// <summary>
        /// Все разделы
        /// </summary>
        public List<Sections> Sections
        {
            get => _Sections;
            set => Set(ref _Sections, value);
        }

        private Sections _SelectedSection;
        /// <summary>
        /// Выбранный раздел
        /// </summary>
        public Sections SelectedSection
        {
            get => _SelectedSection;
            set => Set(ref _SelectedSection, value);
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public OfferInitViewModel(IRepository<Sections> repaSections)
        {
            Sections = repaSections.Items.ToList();
        }
        #endregion
    }
}
