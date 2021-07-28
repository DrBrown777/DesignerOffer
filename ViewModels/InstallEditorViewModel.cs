using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace Designer_Offer.ViewModels
{
    internal class InstallEditorViewModel : ViewModel
    {
        #region СВОЙСТВА
        private string _Name;
        /// <summary>
        /// Название услуги
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private decimal _EntryPrice;
        /// <summary>
        /// Входящая цена услуги
        /// </summary>
        public decimal EntryPrice
        {
            get => _EntryPrice;
            set => Set(ref _EntryPrice, value);
        }

        private List<Unit> _Units;
        /// <summary>
        /// Список всех ед.измерения
        /// </summary>
        public List<Unit> Units
        {
            get => _Units;
            set => Set(ref _Units, value);
        }

        private Unit _SelectedUnit;
        /// <summary>
        /// Выбранная ед. измерения для услуги
        /// </summary>
        public Unit SelectedUnit
        {
            get => _SelectedUnit;
            set => Set(ref _SelectedUnit, value);
        }

        private List<Category> _Categories;
        /// <summary>
        /// Список всех категорий
        /// </summary>
        public List<Category> Categories
        {
            get => _Categories;
            set => Set(ref _Categories, value);
        }

        private Category _SelectedCategory;
        /// <summary>
        /// Выбранная категория для услуги
        /// </summary>
        public Category SelectedCategory
        {
            get => _SelectedCategory;
            set => Set(ref _SelectedCategory, value);
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public InstallEditorViewModel(IRepository<Unit> repaUnit, IRepository<Category> repaCategories)
        {
            Units = repaUnit.Items.ToList();
            Categories = repaCategories.Items.ToList();
        }
        #endregion
    }
}
