using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            set => Set(ref _Products, value);
        }

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
            set => Set(ref _Installs, value);
        }

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
            set => Set(ref _Suppliers, value);
        }

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
            set => Set(ref _Categories, value);
        }

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

        #endregion

        #region МЕТОДЫ

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
        }
        #endregion
    }
}
