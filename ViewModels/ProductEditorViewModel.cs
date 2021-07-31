using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class ProductEditorViewModel : ViewModel
    {
        #region СВОЙСТВА
        private string _Name;
        /// <summary>
        /// Название товара
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private string _Model;
        /// <summary>
        /// Модель товара
        /// </summary>
        public string Model
        {
            get => _Model;
            set => Set(ref _Model, value);
        }

        private decimal _EntryPrice;
        /// <summary>
        /// Входящая цена товара
        /// </summary>
        public decimal EntryPrice
        {
            get => _EntryPrice;
            set => Set(ref _EntryPrice, value);
        }

        private ICollection<Supplier> _ProductSuppliers;
        /// <summary>
        /// Поставщики конкретного товара
        /// </summary>
        public ICollection<Supplier> ProductSuppliers
        {
            get => _ProductSuppliers;
            set => Set(ref _ProductSuppliers, value);
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
        /// Выбранная ед. измерения для товара
        /// </summary>
        public Unit SelectedUnit
        {
            get => _SelectedUnit;
            set => Set(ref _SelectedUnit, value);
        }

        private List<Supplier> _Suppliers;
        /// <summary>
        /// Список всех поставщиков
        /// </summary>
        public List<Supplier> Suppliers
        {
            get => _Suppliers;
            set => Set(ref _Suppliers, value);
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
        /// Выбранная категория конкретного товара
        /// </summary>
        public Category SelectedCategory
        {
            get => _SelectedCategory;
            set => Set(ref _SelectedCategory, value);
        }
        private List<Manufacturer> _Manufacturers;
        /// <summary>
        /// Список всех производителей
        /// </summary>
        public List<Manufacturer> Manufacturers
        {
            get => _Manufacturers;
            set => Set(ref _Manufacturers, value);
        }

        private Manufacturer _SelectedManufacturer;
        /// <summary>
        /// Выбранный производитель конкретного товара
        /// </summary>
        public Manufacturer SelectedManufacturer
        {
            get => _SelectedManufacturer;
            set => Set(ref _SelectedManufacturer, value);
        }
        #endregion

        #region КОМАНДЫ
        /// <summary>
        /// Определение поставщиков для товара
        /// </summary>
        public ICommand AddSuppliers { get; }

        private bool CanAddSuppliers(object p) => true && p != null;

        private void OnAddSuppliers(object p)
        {
            ListBox listBox = (ListBox)p;

            ProductSuppliers?.Clear();

            foreach (Supplier item in listBox.SelectedItems)
            {
                ProductSuppliers.Add(item);
            }
        }

        /// <summary>
        /// Автоматическая Выборка поставщиков у редактируемого товара
        /// </summary>
        public ICommand ChoiceSuppliers { get; }

        private bool CanChoiceSuppliers(object p) => true && p != null;

        private void OnChoiceSuppliers(object p)
        {
            ListBox listBox = (ListBox)p;

            foreach (Supplier item in listBox.Items)
            {
                if (ProductSuppliers.Contains(item))
                {
                    listBox.SelectedItems.Add(item);
                }
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public ProductEditorViewModel(IRepository<Unit> repaUnit,
                                      IRepository<Supplier> repaSuppliers,
                                      IRepository<Category> repaCategories,
                                      IRepository<Manufacturer> repaManufacturers)
        {
            Units = repaUnit.Items.ToList();
            Suppliers = repaSuppliers.Items.ToList();
            Categories = repaCategories.Items.ToList();
            Manufacturers = repaManufacturers.Items.ToList();
            ProductSuppliers = new List<Supplier>();

            ChoiceSuppliers = new LambdaCommand(OnChoiceSuppliers, CanChoiceSuppliers);
            AddSuppliers = new LambdaCommand(OnAddSuppliers, CanAddSuppliers);
        }
        #endregion
    }
}
