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

        private ICollection<Suppliers> _ProductSuppliers;
        /// <summary>
        /// Поставщики конкретного товара
        /// </summary>
        public ICollection<Suppliers> ProductSuppliers
        {
            get => _ProductSuppliers;
            set => Set(ref _ProductSuppliers, value);
        }

        private List<Units> _Units;
        /// <summary>
        /// Список всех ед.измерения
        /// </summary>
        public List<Units> Units
        {
            get => _Units;
            set => Set(ref _Units, value);
        }

        private Units _SelectedUnit;
        /// <summary>
        /// Выбранная ед. измерения для товара
        /// </summary>
        public Units SelectedUnit
        {
            get => _SelectedUnit;
            set => Set(ref _SelectedUnit, value);
        }

        private List<Suppliers> _Suppliers;
        /// <summary>
        /// Список всех поставщиков
        /// </summary>
        public List<Suppliers> Suppliers
        {
            get => _Suppliers;
            set => Set(ref _Suppliers, value);
        }

        private List<Categories> _Categories;
        /// <summary>
        /// Список всех категорий
        /// </summary>
        public List<Categories> Categories
        {
            get => _Categories;
            set => Set(ref _Categories, value);
        }

        private Categories _SelectedCategory;
        /// <summary>
        /// Выбранная категория конкретного товара
        /// </summary>
        public Categories SelectedCategory
        {
            get => _SelectedCategory;
            set => Set(ref _SelectedCategory, value);
        }
        private List<Manufacturers> _Manufacturers;
        /// <summary>
        /// Список всех производителей
        /// </summary>
        public List<Manufacturers> Manufacturers
        {
            get => _Manufacturers;
            set => Set(ref _Manufacturers, value);
        }

        private Manufacturers _SelectedManufacturer;
        /// <summary>
        /// Выбранный производитель конкретного товара
        /// </summary>
        public Manufacturers SelectedManufacturer
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

            foreach (Suppliers item in listBox.SelectedItems)
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

            foreach (Suppliers item in listBox.Items)
            {
                if (ProductSuppliers.Contains(item))
                {
                    listBox.SelectedItems.Add(item);
                }
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public ProductEditorViewModel(IRepository<Units> repaUnit,
                                      IRepository<Suppliers> repaSuppliers,
                                      IRepository<Categories> repaCategories,
                                      IRepository<Manufacturers> repaManufacturers)
        {
            Units = repaUnit.Items.ToList();
            Suppliers = repaSuppliers.Items.ToList();
            Categories = repaCategories.Items.ToList();
            Manufacturers = repaManufacturers.Items.ToList();
            ProductSuppliers = new List<Suppliers>();

            ChoiceSuppliers = new LambdaCommand(OnChoiceSuppliers, CanChoiceSuppliers);
            AddSuppliers = new LambdaCommand(OnAddSuppliers, CanAddSuppliers);
        }
        #endregion
    }
}
