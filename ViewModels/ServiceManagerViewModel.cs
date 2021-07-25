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

namespace Designer_Offer.ViewModels
{
    internal class ServiceManagerViewModel : ViewModel
    {
        #region ПОЛЯ
        private static readonly string _title = " :: Управление услугами";
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        private Employee CurrentUser;
        /// <summary>
        /// Текущая компания
        /// </summary>
        private Company CurrentCompany;
        /// <summary>
        /// Сервис диалогов
        /// </summary>
        private readonly IUserDialog UserDialog;

        #region РЕПОЗИТОРИИ
        /// <summary>
        /// Репозиторий пользователей
        /// </summary>
        private readonly IRepository<Employee> RepositoryUsers;
        /// <summary>
        /// Репозиторий материалов
        /// </summary>
        private readonly IRepository<Product> RepositoryProducts;
        /// <summary>
        /// Репозиторий услуг
        /// </summary>
        private readonly IRepository<Install> RepositoryInstalls;
        /// <summary>
        /// Репозиторий поставщиков
        /// </summary>
        private readonly IRepository<Supplier> RepositorySuppliers;
        /// <summary>
        /// Репозиторий единиц измерения
        /// </summary>
        private readonly IRepository<Unit> RepositoryUnits;
        /// <summary>
        /// Репозиторий категорий
        /// </summary>
        private readonly IRepository<Category> RepositoryCategories;
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

        private ObservableCollection<Product> _Products;
        /// <summary>
        /// Коллекция материлов
        /// </summary>
        public ObservableCollection<Product> Products
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
                            new SortDescription(nameof(Product.Name), ListSortDirection.Ascending)
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

        private Product _SelectedProduct;
        /// <summary>
        /// Выбранный товар
        /// </summary>
        public Product SelectedProduct
        {
            get => _SelectedProduct;
            set => Set(ref _SelectedProduct, value);
        }

        private ObservableCollection<Install> _Installs;
        /// <summary>
        /// Коллекция услуг
        /// </summary>
        public ObservableCollection<Install> Installs
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
                            new SortDescription(nameof(Install.Name), ListSortDirection.Ascending)
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

        private Install _SelectedInstall;
        /// <summary>
        /// Выбранная услуга
        /// </summary>
        public Install SelectedInstall
        {
            get => _SelectedInstall;
            set => Set(ref _SelectedInstall, value);
        }

        private ObservableCollection<Supplier> _Suppliers;
        /// <summary>
        /// Коллекция поставшиков
        /// </summary>
        public ObservableCollection<Supplier> Suppliers
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
                            new SortDescription(nameof(Supplier.Name), ListSortDirection.Ascending)
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

        private Supplier _SelectedSupplier;
        /// <summary>
        /// Выбранный поставщик
        /// </summary>
        public Supplier SelectedSupplier
        {
            get => _SelectedSupplier;
            set => Set(ref _SelectedSupplier, value);
        }
        private ObservableCollection<Unit> _Units;
        /// <summary>
        /// Коллекция единиц измерения
        /// </summary>
        public ObservableCollection<Unit> Units
        {
            get => _Units;
            set => Set(ref _Units, value);
        }

        private Unit _SelectedUnit;
        /// <summary>
        /// Выбранные единицы измерения
        /// </summary>
        public Unit SelectedUnit
        {
            get => _SelectedUnit;
            set => Set(ref _SelectedUnit, value);
        }

        private ObservableCollection<Category> _Categories;
        /// <summary>
        /// Коллекция категорий
        /// </summary>
        public ObservableCollection<Category> Categories
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
                            new SortDescription(nameof(Category.Name), ListSortDirection.Ascending)
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

        private Category _SelectedCategory;
        /// <summary>
        /// Выбранная категория
        /// </summary>
        public Category SelectedCategory
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
                || RepositoryUnits == null || RepositoryCategories == null)
            {
                return false;
            }

            return true;
        }

        private async void OnLoadDataFromRepositories(object p)
        {
            try
            {
                //CurrentUser = await RepositoryUsers.GetAsync(App.Host.Services.GetRequiredService<IEntity>().Id);

                CurrentUser = await RepositoryUsers.GetAsync(21);

                Status = CurrentUser.First_Name + " " + CurrentUser.Last_Name;

                CurrentCompany = CurrentUser.Company;

                Title = CurrentCompany?.Name + _title;

                Products = new ObservableCollection<Product>(await RepositoryProducts.Items.ToListAsync());

                Installs = new ObservableCollection<Install>(await RepositoryInstalls.Items.ToListAsync());

                Suppliers = new ObservableCollection<Supplier>(await RepositorySuppliers.Items.ToListAsync());

                Units = new ObservableCollection<Unit>(await RepositoryUnits.Items.ToListAsync());

                Categories = new ObservableCollection<Category>(await RepositoryCategories.Items.ToListAsync());
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
            Unit new_unit = new Unit();

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
            return (Unit)p != null && SelectedUnit != null;
        }

        private void OnEditUnit(object p)
        {
            Unit edit_unit = (Unit)p ?? SelectedUnit;

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
            return (Unit)p != null && SelectedUnit != null;
        }

        private void OnRemoveUnit(object p)
        {
            Unit unit_to_remove = (Unit)p ?? SelectedUnit;

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
            Supplier new_supplier = new Supplier();

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
            return (Supplier)p != null && SelectedSupplier != null;
        }

        private void OnEditSupplier(object p)
        {
            Supplier supplier_to_edit = (Supplier)p ?? SelectedSupplier;

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
            return (Supplier)p != null && SelectedSupplier != null;
        }

        private void OnRemoveSupplier(object p)
        {
            Supplier supplier_to_remove = (Supplier)p ?? SelectedSupplier;

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
            Category new_category = new Category();

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
            return (Category)p != null && SelectedCategory != null;
        }

        public void OnEditCategory(object p)
        {
            Category category_to_edit = (Category)p ?? SelectedCategory;

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
            return (Category)p != null && SelectedCategory != null;
        }

        public void OnRemoveCategory(object p)
        {
            Category category_to_remove = (Category)p ?? SelectedCategory;

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
        #endregion

        #endregion

        #region МЕТОДЫ
        /// <summary>
        /// Метод фильтрации поставщиков
        /// </summary>
        private void SuppliersViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Supplier supplier) || string.IsNullOrEmpty(SupplierFilter))
            {
                return;
            }

            if (!supplier.Name.ToLower().Contains(SupplierFilter.ToLower()))
            {
                e.Accepted = false;
            }
        }
        /// <summary>
        /// Метод фильтрации категорий
        /// </summary>
        private void CategoriesViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Category category) || string.IsNullOrEmpty(CategoryFilter))
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
            if (!(e.Item is Product product) || string.IsNullOrEmpty(ProductFilter))
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
            if (!(e.Item is Install install) || string.IsNullOrEmpty(InstallFilter))
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
            IRepository<Product> repaProducts,
            IRepository<Install> repaInstalls,
            IRepository<Supplier> repaSuppliers,
            IRepository<Unit> repaUnits,
            IRepository<Category> repaCategories,
            IRepository<Employee> repaUser)
        {
            Progress = true;

            RepositoryUsers = repaUser;
            RepositoryProducts = repaProducts;
            RepositoryInstalls = repaInstalls;
            RepositorySuppliers = repaSuppliers;
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
        }
        #endregion
    }
}
