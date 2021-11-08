using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class PartManagerViewModel : ViewModel
    {
        #region ПОЛЯ
        /// <summary>
        /// Репозиторий систем
        /// </summary>
        private readonly IRepository<Part> RepositoryPart;
        /// <summary>
        /// Сервис Диалогов с пользователем
        /// </summary>
        private readonly IUserDialog UserDialog;
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
                foreach (ProductPart item in RepositoryPart.Get((int)p).ProductPart)
                {
                    Products.Add(item);
                }
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
        }
        #endregion

        public ICommand CalculatePrices { get; }

        private bool CanCalculatePrices(object p)
        {
            return SelectedProduct != null && SelectedProduct.Entry_Price != null && SelectedProduct.Amount != null;
        }

        private void OnCalculatePrices(object p)
        {
            decimal magin_product = SelectedProduct.Part.Offer.Config.Margin_Product;

            try
            {
                SelectedProduct.Out_Price = SelectedProduct.Entry_Price * magin_product;
                SelectedProduct.Entry_Summ = SelectedProduct.Amount * SelectedProduct.Entry_Price;
                SelectedProduct.Out_Summ = SelectedProduct.Amount * SelectedProduct.Out_Price;
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
        }

        #endregion

        #region КОНСТРУКТОРЫ
        public PartManagerViewModel(
            IRepository<Part> repaPart,
            IUserDialog userDialog)
        {
            RepositoryPart = repaPart;
            UserDialog = userDialog;

            Products = new ObservableCollection<ProductPart>();

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);
            CalculatePrices = new LambdaCommand(OnCalculatePrices, CanCalculatePrices);
        }
        #endregion
    }
}
