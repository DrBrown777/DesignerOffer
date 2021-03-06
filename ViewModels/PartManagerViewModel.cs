using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Models;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class PartManagerViewModel : ViewModel
    {
        #region ПОЛЯ
        /// <summary>
        /// Репозиторий систем
        /// </summary>
        private readonly IRepository<Parts> RepositoryPart;
        /// <summary>
        /// Сервис Диалогов с пользователем
        /// </summary>
        private readonly IUserDialog UserDialog;
        /// <summary>
        /// Сервис калькуляции цен
        /// </summary>
        private readonly ICalculator CalculatorService;
        #endregion

        #region СВОЙСТВА
        private int _Id;
        /// <summary>
        /// Id системы
        /// </summary>
        public int Id
        {
            get => _Id;
            set => Set(ref _Id, value);
        }

        private string _Name;
        /// <summary>
        /// Название системы
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private TotalProductPrice _TotalProductPrice;
        /// <summary>
        /// Общая стоимость материалов в системе
        /// </summary>
        public TotalProductPrice TotalProductPrice
        {
            get => _TotalProductPrice;
            set => Set(ref _TotalProductPrice, value);
        }

        private ObservableCollection<ProductPart> _Products;
        /// <summary>
        /// Коллекция товаров в системе
        /// </summary>
        public ObservableCollection<ProductPart> Products
        {
            get => _Products;
            set => Set(ref _Products, value);
        }

        private ProductPart _SelectedProduct;
        /// <summary>
        /// Выбранный товар в системе
        /// </summary>
        public ProductPart SelectedProduct
        {
            get => _SelectedProduct;
            set => Set(ref _SelectedProduct, value);
        }

        private TotalInstallPrice _TotalInstallPrice;
        /// <summary>
        /// Общая стоимость работ и админ расходов в системе
        /// </summary>
        public TotalInstallPrice TotalInstallPrice
        {
            get => _TotalInstallPrice;
            set => Set(ref _TotalInstallPrice, value);
        }

        private ObservableCollection<InstallPart> _Installs;
        /// <summary>
        /// Коллекция работ в системе
        /// </summary>
        public ObservableCollection<InstallPart> Installs
        {
            get => _Installs;
            set => Set(ref _Installs, value);
        }

        private InstallPart _SelectedInstall;
        /// <summary>
        /// Выбранная услуга в системе
        /// </summary>
        public InstallPart SelectedInstall
        {
            get => _SelectedInstall;
            set => Set(ref _SelectedInstall, value);
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
            return RepositoryPart != null && p != null;
        }

        private void OnLoadDataFromRepositories(object p)
        {
            try
            {
                foreach (ProductPart item in RepositoryPart.Get((int)p).ProductPart.OrderBy(it => it.Sort_Order))
                {
                    Products.Add(item);
                }

                if (Products.Count != 0)
                    SelectedProduct = Products.First();

                foreach (InstallPart item in RepositoryPart.Get((int)p).InstallPart.OrderBy(it => it.Sort_Order))
                {
                    Installs.Add(item);
                }

                if (Installs.Count != 0)
                    SelectedInstall = Installs.First();
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }

            if (CalculateGeneralPriceProduct.CanExecute(null))
            {
                CalculateGeneralPriceProduct.Execute(null);
            }

            if (CalculateGeneralPriceInstall.CanExecute(null))
            {
                CalculateGeneralPriceInstall.Execute(null);
            }
        }
        #endregion

        #region добавление удаление данных
        /// <summary>
        /// Удаление товара из системы
        /// </summary>
        public ICommand RemoveProduct { get; }

        private bool CanRemoveProduct(object p)
        {
            return p != null && SelectedProduct != null;
        }

        private void OnRemoveProduct(object p)
        {
            ProductPart remove_to_product = (ProductPart)p ?? SelectedProduct;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить товар {remove_to_product.Products.Name}?", "Удаление товара"))
            {
                return;
            }

            try
            {
                Parts CurrentPart = RepositoryPart.Get(Id);

                CurrentPart.ProductPart.Remove(remove_to_product);

                Products.Remove(remove_to_product);

                UpdateSortOrder(Products);

                RepositoryPart.Update(CurrentPart);

                if (Products.Count != 0)
                {
                    SelectedProduct = Products.First();
                }
                else
                {
                    TotalProductPrice = null;
                }
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }

            if (CalculateGeneralPriceProduct.CanExecute(null))
            {
                CalculateGeneralPriceProduct.Execute(null);
            }
        }
        /// <summary>
        /// Удаление услуги из системы
        /// </summary>
        public ICommand RemoveInstall { get; }

        private bool CanRemoveInstall(object p)
        {
            return p != null && SelectedInstall != null;
        }

        private void OnRemoveInstall(object p)
        {
            InstallPart remove_to_install = (InstallPart)p ?? SelectedInstall;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить услугу {remove_to_install.Installs.Name}?", "Удаление услуги"))
            {
                return;
            }

            try
            {
                Parts CurrentPart = RepositoryPart.Get(Id);

                CurrentPart.InstallPart.Remove(remove_to_install);

                Installs.Remove(remove_to_install);

                UpdateSortOrder(Installs);

                RepositoryPart.Update(CurrentPart);

                if (Installs.Count != 0)
                {
                    SelectedInstall = Installs.First();
                }
                else
                {
                    TotalInstallPrice = null;
                }
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }

            if (CalculateGeneralPriceInstall.CanExecute(null))
            {
                CalculateGeneralPriceInstall.Execute(null);
            }
        }
        #endregion

        #region калькуляция цен
        /// <summary>
        /// Расчет цены в строке по 1-й позиции товара
        /// </summary>
        public ICommand CalculatePricesProduct { get; }

        private bool CanCalculatePricesProduct(object p)
        {
            return SelectedProduct != null && SelectedProduct.Entry_Price != null && SelectedProduct.Amount != null;
        }

        private void OnCalculatePricesProduct(object p)
        {
            try
            {
                CalculatorService.PriceOneItem(SelectedProduct);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }

            if (CalculateGeneralPriceProduct.CanExecute(null))
            {
                CalculateGeneralPriceProduct.Execute(null);
            }
        }
        /// <summary>
        /// Расчет цены в строке по 1-й позиции услуги
        /// </summary>
        public ICommand CalculatePricesInstall { get; }

        private bool CanCalculatePricesInstall(object p)
        {
            return SelectedInstall != null && SelectedInstall.Entry_Price != null && SelectedInstall.Amount != null;
        }

        private void OnCalculatePricesInstall(object p)
        {
            try
            {
                CalculatorService.PriceOneItem(SelectedInstall);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }

            if (CalculateGeneralPriceInstall.CanExecute(null))
            {
                CalculateGeneralPriceInstall.Execute(null);
            }
        }

        /// <summary>
        /// Вычесление итоговой суммы материалов
        /// </summary>
        public ICommand CalculateGeneralPriceProduct { get; }

        private bool CanCalculateGeneralPriceProduct(object p)
        {
            return Products.Count != 0 && CalculatorService != null;
        }

        private void OnCalculateGeneralPriceProduct(object p)
        {
            try
            {
                TotalProductPrice = CalculatorService.CalculateTotalProductPrice(Products);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
        }

        /// <summary>
        /// Вычесление итоговой суммы работ и админ расходов
        /// </summary>
        public ICommand CalculateGeneralPriceInstall { get; }

        private bool CanCalculateGeneralPriceInstall(object p)
        {
            return Installs.Count != 0 && CalculatorService != null;
        }

        private void OnCalculateGeneralPriceInstall(object p)
        {
            try
            {
                TotalInstallPrice = CalculatorService.CalculateTotalInstallPrice(Installs);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
        }
        #endregion

        #region сортировка позиций
        /// <summary>
        /// Пользовательская сортировка товаров в системе
        /// </summary>
        public ICommand SwappingElementProduct { get; }

        private bool CanSwappingElementProduct(object p)
        {
            bool res = bool.Parse((string)p);

            int index = Products.IndexOf(SelectedProduct);

            return SelectedProduct != null &&
                !((index + 1) == Products.Count && res) &&
                !((index - 1) < 0 && res == false);
        }

        private void OnSwappingElementProduct(object p)
        {
            bool res = bool.Parse((string)p);

            int index = Products.IndexOf(SelectedProduct);

            if (res)
            {
                Products.Move(index, index + 1);
                UpdateSortOrder(Products);
                return;
            }

            Products.Move(index, index - 1);
            UpdateSortOrder(Products);
        }
        /// <summary>
        /// Пользовательская сортировка услуг в системе
        /// </summary>
        public ICommand SwappingElementInstall { get; }

        private bool CanSwappingElementInstall(object p)
        {
            bool res = bool.Parse((string)p);

            int index = Installs.IndexOf(SelectedInstall);

            return SelectedInstall != null &&
                !((index + 1) == Installs.Count && res) &&
                !((index - 1) < 0 && res == false);
        }

        private void OnSwappingElementInstall(object p)
        {
            bool res = bool.Parse((string)p);

            int index = Installs.IndexOf(SelectedInstall);

            if (res)
            {
                Installs.Move(index, index + 1);
                UpdateSortOrder(Installs);
                return;
            }

            Installs.Move(index, index - 1);
            UpdateSortOrder(Installs);
        }
        #endregion

        #endregion

        #region МЕТОДЫ
        /// <summary>
        /// Обновление в коллекции индекса сортировки
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        private void UpdateSortOrder<T>(ObservableCollection<T> collection)
        {
            foreach (T item in collection)
            {
                int index = collection.IndexOf(item);

                if (item is ProductPart product)
                {
                    product.Sort_Order = index;
                }

                if (item is InstallPart install)
                {
                    install.Sort_Order = index;
                }
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public PartManagerViewModel(
            IRepository<Parts> repaPart,
            IUserDialog userDialog,
            ICalculator calcService)
        {
            RepositoryPart = repaPart;
            UserDialog = userDialog;
            CalculatorService = calcService;

            Products = new ObservableCollection<ProductPart>();
            Installs = new ObservableCollection<InstallPart>();

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);

            CalculatePricesProduct = new LambdaCommand(OnCalculatePricesProduct, CanCalculatePricesProduct);
            CalculatePricesInstall = new LambdaCommand(OnCalculatePricesInstall, CanCalculatePricesInstall);
            CalculateGeneralPriceProduct = new LambdaCommand(OnCalculateGeneralPriceProduct, CanCalculateGeneralPriceProduct);
            CalculateGeneralPriceInstall = new LambdaCommand(OnCalculateGeneralPriceInstall, CanCalculateGeneralPriceInstall);

            SwappingElementProduct = new LambdaCommand(OnSwappingElementProduct, CanSwappingElementProduct);
            SwappingElementInstall = new LambdaCommand(OnSwappingElementInstall, CanSwappingElementInstall);

            RemoveProduct = new LambdaCommand(OnRemoveProduct, CanRemoveProduct);
            RemoveInstall = new LambdaCommand(OnRemoveInstall, CanRemoveInstall);
        }
        #endregion
    }
}
