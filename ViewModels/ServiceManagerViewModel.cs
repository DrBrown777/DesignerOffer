using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;

namespace Designer_Offer.ViewModels
{
    internal class ServiceManagerViewModel : ViewModel
    {
        #region ПОЛЯ
        private static readonly string _title = " :: Управление услугами";

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private Employees CurrentUser;

        /// <summary>
        /// Текущая компания
        /// </summary>
        private Companies CurrentCompany;

        /// <summary>
        /// Сервис диалогов
        /// </summary>
        private readonly IUserDialog UserDialog;

        #region РЕПОЗИТОРИИ
        /// <summary>
        /// Репозиторий пользователей
        /// </summary>
        private readonly IRepository<Employees> RepositoryUsers;
        /// <summary>
        /// Репозиторий материалов
        /// </summary>
        private readonly IRepository<Products> RepositoryProducts;
        /// <summary>
        /// Репозиторий услуг
        /// </summary>
        private readonly IRepository<Installs> RepositoryInstalls;
        /// <summary>
        /// Репозиторий поставщиков
        /// </summary>
        private readonly IRepository<Suppliers> RepositorySuppliers;
        /// <summary>
        /// Репозиторий единиц измерения
        /// </summary>
        private readonly IRepository<Units> RepositoryUnits;
        /// <summary>
        /// Репозиторий категорий
        /// </summary>
        private readonly IRepository<Categories> RepositoryCategories;
        /// <summary>
        /// Репозиторий производителей
        /// </summary>
        private readonly IRepository<Manufacturers> RepositoryManufacturers; 
        #endregion

        #endregion

        #region СВОЙСТВА
        private string _Title;
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

        private bool _Progress;
        /// <summary>
        /// Индикатор прогрессбара
        /// </summary>
        public bool Progress
        {
            get => _Progress;
            set => Set(ref _Progress, value);
        }

        private ObservableCollection<Products> _Products;
        /// <summary>
        /// Коллекция материлов
        /// </summary>
        public ObservableCollection<Products> Products
        {
            get => _Products;
            set
            {
                if (Set(ref _Products, value))
                {
                    {
                        ProductsViewSource = new CollectionViewSource()
                        {
                            Source = value,
                            SortDescriptions =
                        {
                            new SortDescription(nameof(Data.Products.Name), ListSortDirection.Ascending)
                        }

                        };
                        ProductsViewSource.Filter += ProductsViewSource_Filter;
                        ProductsViewSource.View.Refresh();

                        OnPropertyChanged(nameof(ProductsView));
                    }
                }
            }
        }

        private string _ProductFilter;
        /// <summary>
        /// Искомый товар для фильтрации
        /// </summary>
        public string ProductFilter
        {
            get => _ProductFilter;
            set
            {
                if (Set(ref _ProductFilter, value))
                {
                    ProductsViewSource?.View.Refresh();
                }
            }
        }
        /// <summary>
        /// Пользователская сортировка товаров
        /// </summary>
        public ICollectionView ProductsView => ProductsViewSource?.View;

        /// <summary>
        /// Прокси коллекция товаров
        /// </summary>
        private CollectionViewSource ProductsViewSource;

        private Products _SelectedProduct;
        /// <summary>
        /// Выбранный товар
        /// </summary>
        public Products SelectedProduct
        {
            get => _SelectedProduct;
            set => Set(ref _SelectedProduct, value);
        }

        private ObservableCollection<Installs> _Installs;
        /// <summary>
        /// Коллекция услуг
        /// </summary>
        public ObservableCollection<Installs> Installs
        {
            get => _Installs;
            set
            {
                if (Set(ref _Installs, value))
                {
                    {
                        InstallsViewSource = new CollectionViewSource()
                        {
                            Source = value,
                            SortDescriptions =
                        {
                            new SortDescription(nameof(Data.Installs.Name), ListSortDirection.Ascending)
                        }

                        };
                        InstallsViewSource.Filter += InstallsViewSource_Filter;
                        InstallsViewSource.View.Refresh();

                        OnPropertyChanged(nameof(InstallsView));
                    }
                }
            }
        }

        private string _InstallFilter;
        /// <summary>
        /// Искомая услуга для фильтрации
        /// </summary>
        public string InstallFilter
        {
            get => _InstallFilter;
            set
            {
                if (Set(ref _InstallFilter, value))
                {
                    InstallsViewSource?.View.Refresh();
                }
            }
        }
        /// <summary>
        /// Пользователская сортировка услуг
        /// </summary>
        public ICollectionView InstallsView => InstallsViewSource?.View;

        /// <summary>
        /// Прокси коллекция услуг
        /// </summary>
        private CollectionViewSource InstallsViewSource;

        private Installs _SelectedInstall;
        /// <summary>
        /// Выбранная услуга
        /// </summary>
        public Installs SelectedInstall
        {
            get => _SelectedInstall;
            set => Set(ref _SelectedInstall, value);
        }

        private ObservableCollection<Suppliers> _Suppliers;
        /// <summary>
        /// Коллекция поставшиков
        /// </summary>
        public ObservableCollection<Suppliers> Suppliers
        {
            get => _Suppliers;
            set
            {
                if (Set(ref _Suppliers, value))
                {
                    {
                        SuppliersViewSource = new CollectionViewSource()
                        {
                            Source = value,
                            SortDescriptions =
                        {
                            new SortDescription(nameof(Data.Suppliers.Name), ListSortDirection.Ascending)
                        }

                        };
                        SuppliersViewSource.Filter += SuppliersViewSource_Filter;
                        SuppliersViewSource.View.Refresh();

                        OnPropertyChanged(nameof(SuppliersView));
                    }
                }
            }
        }

        private string _SupplierFilter;
        /// <summary>
        /// Искомый поставщик для фильтрации
        /// </summary>
        public string SupplierFilter
        {
            get => _SupplierFilter;
            set
            {
                if (Set(ref _SupplierFilter, value))
                {
                    SuppliersViewSource?.View.Refresh();
                }
            }
        }
        /// <summary>
        /// Пользователская сортировка поставщиков
        /// </summary>
        public ICollectionView SuppliersView => SuppliersViewSource?.View;

        /// <summary>
        /// Прокси коллекция поставщиков
        /// </summary>
        private CollectionViewSource SuppliersViewSource;

        private Suppliers _SelectedSupplier;
        /// <summary>
        /// Выбранный поставщик
        /// </summary>
        public Suppliers SelectedSupplier
        {
            get => _SelectedSupplier;
            set => Set(ref _SelectedSupplier, value);
        }

        private ObservableCollection<Manufacturers> _Manufacturers;
        /// <summary>
        /// Коллекция производителей
        /// </summary>
        public ObservableCollection<Manufacturers> Manufacturers
        {
            get => _Manufacturers;
            set
            {
                if (Set(ref _Manufacturers, value))
                {
                    {
                        ManufacturersViewSource = new CollectionViewSource()
                        {
                            Source = value,
                            SortDescriptions =
                        {
                            new SortDescription(nameof(Data.Manufacturers.Name), ListSortDirection.Ascending)
                        }

                        };
                        ManufacturersViewSource.Filter += ManufacturersViewSource_Filter;
                        ManufacturersViewSource.View.Refresh();

                        OnPropertyChanged(nameof(ManufacturersView));
                    }
                }
            }
        }
        private string _ManufacturerFilter;
        /// <summary>
        /// Искомый производитель для фильтрации
        /// </summary>
        public string ManufacturerFilter
        {
            get => _ManufacturerFilter;
            set
            {
                if (Set(ref _ManufacturerFilter, value))
                {
                    ManufacturersViewSource?.View.Refresh();
                }
            }
        }
        /// <summary>
        /// Пользователская сортировка производителей
        /// </summary>
        public ICollectionView ManufacturersView => ManufacturersViewSource?.View;

        /// <summary>
        /// Прокси коллекция производителей
        /// </summary>
        private CollectionViewSource ManufacturersViewSource;

        private Manufacturers _SelectedManufacturer;
        /// <summary>
        /// Выбранный производитель
        /// </summary>
        public Manufacturers SelectedManufacturer
        {
            get => _SelectedManufacturer;
            set => Set(ref _SelectedManufacturer, value);
        }

        private ObservableCollection<Units> _Units;
        /// <summary>
        /// Коллекция единиц измерения
        /// </summary>
        public ObservableCollection<Units> Units
        {
            get => _Units;
            set => Set(ref _Units, value);
        }

        private Units _SelectedUnit;
        /// <summary>
        /// Выбранные единицы измерения
        /// </summary>
        public Units SelectedUnit
        {
            get => _SelectedUnit;
            set => Set(ref _SelectedUnit, value);
        }

        private ObservableCollection<Categories> _Categories;
        /// <summary>
        /// Коллекция категорий
        /// </summary>
        public ObservableCollection<Categories> Categories
        {
            get => _Categories;
            set
            {
                if (Set(ref _Categories, value))
                {
                    {
                        CategoriesViewSource = new CollectionViewSource()
                        {
                            Source = value,
                            SortDescriptions =
                        {
                            new SortDescription(nameof(Data.Categories.Name), ListSortDirection.Ascending)
                        }

                        };
                        CategoriesViewSource.Filter += CategoriesViewSource_Filter;
                        CategoriesViewSource.View.Refresh();

                        OnPropertyChanged(nameof(CategoriesView));
                    }
                }
            }
        }

        private string _CategoryFilter;
        /// <summary>
        /// Искомая категория для фильтрации
        /// </summary>
        public string CategoryFilter
        {
            get => _CategoryFilter;
            set
            {
                if (Set(ref _CategoryFilter, value))
                {
                    CategoriesViewSource?.View.Refresh();
                }
            }
        }
        /// <summary>
        /// Пользователская сортировка категорий
        /// </summary>
        public ICollectionView CategoriesView => CategoriesViewSource?.View;

        /// <summary>
        /// Прокси коллекция категорий
        /// </summary>
        private CollectionViewSource CategoriesViewSource;

        private Categories _SelectedCategory;
        /// <summary>
        /// Выбранная категория
        /// </summary>
        public Categories SelectedCategory
        {
            get => _SelectedCategory;
            set => Set(ref _SelectedCategory, value);
        }
        #endregion

        #region КОМАНДЫ

        #region загрузка данных из репозиториев
        /// <summary>
        /// Загрузка данных из репозиториев
        /// </summary>
        public ICommand LoadDataFromRepositories { get; }

        private bool CanLoadDataFromRepositories(object p)
        {
            if (RepositoryUsers == null || RepositoryProducts == null
                || RepositoryInstalls == null || RepositorySuppliers == null
                || RepositoryUnits == null || RepositoryCategories == null
                || RepositoryManufacturers == null)
            {
                return false;
            }

            return true;
        }

        private async void OnLoadDataFromRepositories(object p)
        {
            try
            {
                //CurrentUser = await RepositoryUsers.GetAsync(App.Host.Services.GetRequiredService<Employee>().Id);

                CurrentUser = await RepositoryUsers.GetAsync(21);

                Status = CurrentUser.First_Name + " " + CurrentUser.Last_Name;

                CurrentCompany = CurrentUser.Companies;

                Title = CurrentCompany?.Name + _title;

                Products = new ObservableCollection<Products>(await RepositoryProducts.Items.ToListAsync());

                Installs = new ObservableCollection<Installs>(await RepositoryInstalls.Items.ToListAsync());

                Suppliers = new ObservableCollection<Suppliers>(await RepositorySuppliers.Items.ToListAsync());

                Manufacturers = new ObservableCollection<Manufacturers>(await RepositoryManufacturers.Items.ToListAsync());

                Units = new ObservableCollection<Units>(await RepositoryUnits.Items.ToListAsync());

                Categories = new ObservableCollection<Categories>(await RepositoryCategories.Items.ToListAsync());
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                Progress = false;
            }
        }
        #endregion

        #region добавление/удаление данных
        /// <summary>
        /// Добавить новые ед.измерения
        /// </summary>
        public ICommand AddNewUnit { get; }

        private bool CanAddNewUnit(object p) => true;

        private void OnAddNewUnit(object p)
        {
            Units new_unit = new Units();

            if (!UserDialog.Edit(new_unit))
            {
                return;
            }

            try
            {
                Units.Add(RepositoryUnits.Add(new_unit));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedUnit = new_unit;
            }
        }
        /// <summary>
        /// Редактирование ед.измерения
        /// </summary>
        public ICommand EditUnit { get; }

        private bool CanEditUnit(object p)
        {
            return (Units)p != null && SelectedUnit != null;
        }

        private void OnEditUnit(object p)
        {
            Units edit_unit = (Units)p ?? SelectedUnit;

            if (!UserDialog.Edit(edit_unit))
            {
                return;
            }

            try
            {
                RepositoryUnits.Update(edit_unit);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                if (Units.Remove(edit_unit))
                {
                    Units.Add(edit_unit);
                }

                SelectedUnit = edit_unit;
            }
        }
        /// <summary>
        /// Удаление ед.измерения
        /// </summary>
        public ICommand RemoveUnit { get; }

        private bool CanRemoveUnit(object p)
        {
            return (Units)p != null && SelectedUnit != null;
        }

        private void OnRemoveUnit(object p)
        {
            Units unit_to_remove = (Units)p ?? SelectedUnit;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить ед.измерения {unit_to_remove.Name}?", "Удаление ед.измерения"))
            {
                return;
            }

            try
            {
                RepositoryUnits.Remove(unit_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                Units.Remove(unit_to_remove);

                if (ReferenceEquals(SelectedUnit, unit_to_remove))
                {
                    SelectedUnit = null;
                }
            }
        }
        /// <summary>
        /// Добавление нового поставщика
        /// </summary>
        public ICommand AddNewSupplier { get; }

        private bool CanAddNewSupplier(object p) => true;

        private void OnAddNewSupplier(object p)
        {
            Suppliers new_supplier = new Suppliers();

            if (!UserDialog.Edit(new_supplier))
            {
                return;
            }

            try
            {
                Suppliers.Add(RepositorySuppliers.Add(new_supplier));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedSupplier = new_supplier;
            }
        }
        /// <summary>
        /// Редактирование поставщика
        /// </summary>
        public ICommand EditSupplier { get; }

        private bool CanEditSupplier(object p)
        {
            return (Suppliers)p != null && SelectedSupplier != null;
        }

        private void OnEditSupplier(object p)
        {
            Suppliers supplier_to_edit = (Suppliers)p ?? SelectedSupplier;

            if (!UserDialog.Edit(supplier_to_edit))
            {
                return;
            }

            try
            {
                RepositorySuppliers.Update(supplier_to_edit);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SuppliersViewSource.View.Refresh();

                SelectedSupplier = supplier_to_edit;
            }
        }

        /// <summary>
        /// Удаление поставщика
        /// </summary>
        public ICommand RemoveSupplier { get; }

        private bool CanRemoveSupplier(object p)
        {
            return (Suppliers)p != null && SelectedSupplier != null;
        }

        private void OnRemoveSupplier(object p)
        {
            Suppliers supplier_to_remove = (Suppliers)p ?? SelectedSupplier;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить поставщика {supplier_to_remove.Name}?", "Удаление поставщика"))
            {
                return;
            }

            try
            {
                RepositorySuppliers.Remove(supplier_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                if (ReferenceEquals(supplier_to_remove, SelectedSupplier))
                {
                    SelectedSupplier = null;
                }

                Suppliers.Remove(supplier_to_remove);
            }
        }
        /// <summary>
        /// Добавление новой категории
        /// </summary>
        public ICommand AddNewCategory { get; }

        public bool CanAddNewCategory(object p) => true;

        public void OnAddNewCategory(object p)
        {
            Categories new_category = new Categories();

            if (!UserDialog.Edit(new_category))
            {
                return;
            }

            try
            {
                Categories.Add(RepositoryCategories.Add(new_category));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedCategory = new_category;
            }
        }
        /// <summary>
        /// Редактирование категории
        /// </summary>
        public ICommand EditCategory { get; }

        public bool CanEditCategory(object p)
        {
            return (Categories)p != null && SelectedCategory != null;
        }

        public void OnEditCategory(object p)
        {
            Categories category_to_edit = (Categories)p ?? SelectedCategory;

            if (!UserDialog.Edit(category_to_edit))
            {
                return;
            }

            try
            {
                RepositoryCategories.Update(category_to_edit);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                CategoriesViewSource.View.Refresh();

                SelectedCategory = category_to_edit;
            }
        }
        /// <summary>
        /// Удаление категории
        /// </summary>
        public ICommand RemoveCategory { get; }

        public bool CanRemoveCategory(object p)
        {
            return (Categories)p != null && SelectedCategory != null;
        }

        public void OnRemoveCategory(object p)
        {
            Categories category_to_remove = (Categories)p ?? SelectedCategory;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить категорию {category_to_remove.Name}?", "Удаление категории"))
            {
                return;
            }

            try
            {
                RepositoryCategories.Remove(category_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                if (ReferenceEquals(SelectedCategory, category_to_remove))
                {
                    SelectedCategory = null;
                }

                Categories.Remove(category_to_remove);
            }
        }
        /// <summary>
        /// Добавление нового товара
        /// </summary>
        public ICommand AddNewProduct { get; }

        private bool CanAddNewProduct(object p) => true;

        private void OnAddNewProduct(object p)
        {
            Products new_product = new Products
            {
                Entry_Price = 0
            };

            if (!UserDialog.Edit(new_product))
            {
                return;
            }

            try
            {
                Products.Add(RepositoryProducts.Add(new_product));

                foreach (var item in new_product.Suppliers)
                {
                    Suppliers.SingleOrDefault(s => s.Id == item.Id)?.Products.Add(new_product);
                }

                Manufacturers.SingleOrDefault(m => m.Id == new_product.Manufacturer_Id)?.Products.Add(new_product);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SuppliersViewSource.View.Refresh();

                ManufacturersViewSource.View.Refresh();

                OnPropertyChanged(nameof(SuppliersView));

                OnPropertyChanged(nameof(ManufacturersView));

                SelectedProduct = new_product;
            }
        }
        /// <summary>
        /// Редактирование товара
        /// </summary>
        public ICommand EditProduct { get; }

        private bool CanEditProduct(object p)
        {
            return (Products)p != null && SelectedProduct != null;
        }

        private void OnEditProduct(object p)
        {
            Products product_to_edit = (Products)p ?? SelectedProduct;

            if (!UserDialog.Edit(product_to_edit))
            {
                return;
            }

            try
            {
                RepositoryProducts.Update(product_to_edit);

                foreach (var item in product_to_edit.Suppliers)
                {
                    Suppliers.SingleOrDefault(s => ReferenceEquals(item.Products, product_to_edit))?.Products.Remove(product_to_edit);

                    Suppliers.SingleOrDefault(s => s.Id == item.Id)?.Products.Add(product_to_edit);
                }

                Manufacturers.SingleOrDefault(m => ReferenceEquals(product_to_edit, m.Products))?.Products.Remove(product_to_edit);

                Manufacturers.SingleOrDefault(m => m.Id == product_to_edit.Manufacturer_Id)?.Products.Add(product_to_edit);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                ProductsViewSource.View.Refresh();

                SuppliersViewSource.View.Refresh();

                ManufacturersViewSource.View.Refresh();

                OnPropertyChanged(nameof(SuppliersView));

                OnPropertyChanged(nameof(ManufacturersView));

                SelectedProduct = product_to_edit;
            }
        }
        /// <summary>
        /// Удаление товара
        /// </summary>
        public ICommand RemoveProduct { get; }

        private bool CanRemoveProduct(object p)
        {
            return (Products)p != null && SelectedProduct != null;
        }

        private void OnRemoveProduct(object p)
        {
            Products product_to_remove = (Products)p ?? SelectedProduct;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить товар {product_to_remove.Name}?", "Удаление товара"))
            {
                return;
            }

            try
            {
                RepositoryProducts.Remove(product_to_remove.Id);

                foreach (var item in product_to_remove.Suppliers)
                {
                    Suppliers.SingleOrDefault(s => ReferenceEquals(item.Products, product_to_remove))?.Products.Remove(product_to_remove);
                }

                Manufacturers.SingleOrDefault(m => ReferenceEquals(product_to_remove, m.Products))?.Products.Remove(product_to_remove);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SuppliersViewSource.View.Refresh();

                ManufacturersViewSource.View.Refresh();

                OnPropertyChanged(nameof(SuppliersView));

                OnPropertyChanged(nameof(ManufacturersView));

                if (ReferenceEquals(SelectedProduct, product_to_remove))
                {
                    SelectedProduct = null;
                }

                Products.Remove(product_to_remove);
            }
        }
        /// <summary>
        /// Добавление новой услуги
        /// </summary>
        public ICommand AddNewInstall { get; }

        private bool CanAddNewInstall(object p) => true;

        private void OnAddNewInstall(object p)
        {
            Installs new_install = new Installs
            {
                Entry_Price = 0
            };

            if (!UserDialog.Edit(new_install))
            {
                return;
            }

            try
            {
                Installs.Add(RepositoryInstalls.Add(new_install));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedInstall = new_install;
            }
        }
        /// <summary>
        /// Редактирование услуги
        /// </summary>
        public ICommand EditInstall { get; }

        private bool CanEditInstall(object p)
        {
            return (Installs)p != null && SelectedInstall != null;
        }

        private void OnEditInstall(object p)
        {
            Installs install_to_edit = (Installs)p ?? SelectedInstall;

            if (!UserDialog.Edit(install_to_edit))
            {
                return;
            }

            try
            {
                RepositoryInstalls.Update(install_to_edit);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                InstallsViewSource.View.Refresh();

                SelectedInstall = install_to_edit;
            }
        }
        /// <summary>
        /// Удаление услуги
        /// </summary>
        public ICommand RemoveInstall { get; }

        private bool CanRemoveInstall(object p)
        {
            return (Installs)p != null && SelectedInstall != null;
        }

        private void OnRemoveInstall(object p)
        {
            Installs install_to_remove = (Installs)p ?? SelectedInstall;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить услугу {install_to_remove.Name}?", "Удаление услуги"))
            {
                return;
            }

            try
            {
                RepositoryInstalls.Remove(install_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                if (ReferenceEquals(install_to_remove, SelectedInstall))
                {
                    SelectedInstall = null;
                }

                Installs.Remove(install_to_remove);
            }
        }
        /// <summary>
        /// Добавление нового поставщика
        /// </summary>
        public ICommand AddNewManufacturer { get; }

        private bool CanAddAddNewManufacturer(object p) => true;

        private void OnAddAddNewManufacturer(object p)
        {
            Manufacturers new_manufacturer = new Manufacturers();

            if (!UserDialog.Edit(new_manufacturer))
            {
                return;
            }

            try
            {
                Manufacturers.Add(RepositoryManufacturers.Add(new_manufacturer));
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                SelectedManufacturer = new_manufacturer;
            }
        }
        /// <summary>
        /// Редактирование поставщика
        /// </summary>
        public ICommand EditManufacturer { get; }

        private bool CanEditManufacturer(object p)
        {
            return (Manufacturers)p != null && SelectedManufacturer != null;
        }

        private void OnEditManufacturer(object p)
        {
            Manufacturers manufacturer_to_edit = (Manufacturers)p ?? SelectedManufacturer;

            if (!UserDialog.Edit(manufacturer_to_edit))
            {
                return;
            }

            try
            {
                RepositoryManufacturers.Update(manufacturer_to_edit);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                ManufacturersViewSource.View.Refresh();

                SelectedManufacturer = manufacturer_to_edit;
            }
        }

        /// <summary>
        /// Удаление поставщика
        /// </summary>
        public ICommand RemoveManufacturer { get; }

        private bool CanRemoveManufacturer(object p)
        {
            return (Manufacturers)p != null && SelectedManufacturer != null;
        }

        private void OnRemoveManufacturer(object p)
        {
            Manufacturers manufacturer_to_remove = (Manufacturers)p ?? SelectedManufacturer;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить производителя {manufacturer_to_remove.Name}?", "Удаление производителя"))
            {
                return;
            }

            try
            {
                RepositoryManufacturers.Remove(manufacturer_to_remove.Id);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            finally
            {
                if (ReferenceEquals(manufacturer_to_remove, SelectedManufacturer))
                {
                    SelectedManufacturer = null;
                }

                Manufacturers.Remove(manufacturer_to_remove);
            }
        }
        #endregion

        #endregion

        #region МЕТОДЫ
        /// <summary>
        /// Метод фильтрации поставщиков
        /// </summary>
        private void SuppliersViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Suppliers supplier) || string.IsNullOrEmpty(SupplierFilter))
            {
                return;
            }

            if (!supplier.Name.ToLower().Contains(SupplierFilter.ToLower()))
            {
                e.Accepted = false;
            }
        }
        /// <summary>
        /// Метод фильтрации производителей
        /// </summary>
        private void ManufacturersViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Manufacturers manufacturer) || string.IsNullOrEmpty(ManufacturerFilter))
            {
                return;
            }

            if (!manufacturer.Name.ToLower().Contains(ManufacturerFilter.ToLower()))
            {
                e.Accepted = false;
            }
        }
        /// <summary>
        /// Метод фильтрации категорий
        /// </summary>
        private void CategoriesViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Categories category) || string.IsNullOrEmpty(CategoryFilter))
            {
                return;
            }

            if (!category.Name.ToLower().Contains(CategoryFilter.ToLower()))
            {
                e.Accepted = false;
            }
        }
        /// <summary>
        /// Метод сортировки товаров
        /// </summary>
        private void ProductsViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Products product) || string.IsNullOrEmpty(ProductFilter))
            {
                return;
            }

            if (!product.Name.ToLower().Contains(ProductFilter.ToLower()))
            {
                e.Accepted = false;
            }
        }
        /// <summary>
        /// Метод сортировки услуг
        /// </summary>
        private void InstallsViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Installs install) || string.IsNullOrEmpty(InstallFilter))
            {
                return;
            }

            if (!install.Name.ToLower().Contains(InstallFilter.ToLower()))
            {
                e.Accepted = false;
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public ServiceManagerViewModel(
            IUserDialog userDialog,
            IRepository<Products> repaProducts,
            IRepository<Installs> repaInstalls,
            IRepository<Suppliers> repaSuppliers,
            IRepository<Units> repaUnits,
            IRepository<Categories> repaCategories,
            IRepository<Employees> repaUser,
            IRepository<Manufacturers> repaManufacturer)
        {
            Progress = true;

            RepositoryUsers = repaUser;
            RepositoryProducts = repaProducts;
            RepositoryInstalls = repaInstalls;
            RepositorySuppliers = repaSuppliers;
            RepositoryManufacturers = repaManufacturer;
            RepositoryUnits = repaUnits;
            RepositoryCategories = repaCategories;

            UserDialog = userDialog;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);

            AddNewUnit = new LambdaCommand(OnAddNewUnit, CanAddNewUnit);
            EditUnit = new LambdaCommand(OnEditUnit, CanEditUnit);
            RemoveUnit = new LambdaCommand(OnRemoveUnit, CanRemoveUnit);

            AddNewSupplier = new LambdaCommand(OnAddNewSupplier, CanAddNewSupplier);
            EditSupplier = new LambdaCommand(OnEditSupplier, CanEditSupplier);
            RemoveSupplier = new LambdaCommand(OnRemoveSupplier, CanRemoveSupplier);

            AddNewCategory = new LambdaCommand(OnAddNewCategory, CanAddNewCategory);
            EditCategory = new LambdaCommand(OnEditCategory, CanEditCategory);
            RemoveCategory = new LambdaCommand(OnRemoveCategory, CanRemoveCategory);

            AddNewProduct = new LambdaCommand(OnAddNewProduct, CanAddNewProduct);
            EditProduct = new LambdaCommand(OnEditProduct, CanEditProduct);
            RemoveProduct = new LambdaCommand(OnRemoveProduct, CanRemoveProduct);

            AddNewInstall = new LambdaCommand(OnAddNewInstall, CanAddNewInstall);
            EditInstall = new LambdaCommand(OnEditInstall, CanEditInstall);
            RemoveInstall = new LambdaCommand(OnRemoveInstall, CanRemoveInstall);

            AddNewManufacturer = new LambdaCommand(OnAddAddNewManufacturer, CanAddAddNewManufacturer);
            EditManufacturer = new LambdaCommand(OnEditManufacturer, CanEditManufacturer);
            RemoveManufacturer = new LambdaCommand(OnRemoveManufacturer, CanRemoveManufacturer);
        }
        #endregion
    }
}
