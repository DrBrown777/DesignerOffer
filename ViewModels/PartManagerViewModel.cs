﻿using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Views.UControl;
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

        private decimal _ProductProfit;
        /// <summary>
        /// Прибыль в % материалов
        /// </summary>
        public decimal ProductProfit
        {
            get => _ProductProfit;
            set => Set(ref _ProductProfit, value);
        }

        private decimal _ProductProceeds;
        /// <summary>
        /// Доход по материалам
        /// </summary>
        public decimal ProductProceeds
        {
            get => _ProductProceeds;
            set => Set(ref _ProductProceeds, value);
        }

        private decimal _ProductEntrySumm;
        /// <summary>
        /// Итоговый вход по материалам
        /// </summary>
        public decimal ProductEntrySumm
        {
            get => _ProductEntrySumm;
            set => Set(ref _ProductEntrySumm, value);
        }

        private decimal _ProductOutSumm;
        /// <summary>
        /// Итоговый выход по материалам
        /// </summary>
        public decimal ProductOutSumm
        {
            get => _ProductOutSumm;
            set => Set(ref _ProductOutSumm, value);
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

            if (CalculateGeneralPrice.CanExecute(null))
            {
                CalculateGeneralPrice.Execute(null);
            }
        }
        #endregion

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

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить товар {remove_to_product.Product.Name}?", "Удаление товара"))
            {
                return;
            }

            try
            {
                Part CurrentPart = RepositoryPart.Get(Id);

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
                    ProductEntrySumm = 0;
                    ProductOutSumm = 0;
                    ProductProceeds = 0;
                    ProductProfit = 0;
                }
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }

            if (CalculateGeneralPrice.CanExecute(null))
            {
                CalculateGeneralPrice.Execute(null);
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

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить услугу {remove_to_install.Install.Name}?", "Удаление услуги"))
            {
                return;
            }

            try
            {
                Part CurrentPart = RepositoryPart.Get(Id);

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
                    /*Обнулить ценники*/
                }
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
            /*
             *дописать пересчет цены, или оставить эту команду
            if (CalculateGeneralPrice.CanExecute(null))
            {
                CalculateGeneralPrice.Execute(null);
            }
            */
        }
        /// <summary>
        /// Расчет цены в строке по 1-й позиции
        /// </summary>
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
                SelectedProduct.Out_Price = RoundDecimal(SelectedProduct.Entry_Price * magin_product);
                SelectedProduct.Entry_Summ = RoundDecimal(SelectedProduct.Amount * SelectedProduct.Entry_Price);
                SelectedProduct.Out_Summ = RoundDecimal(SelectedProduct.Amount * SelectedProduct.Out_Price);

                SelectedProduct = UpdateCollection(Products, SelectedProduct);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }

            if (CalculateGeneralPrice.CanExecute(null))
            {
                CalculateGeneralPrice.Execute(null);
            }
        }

        /// <summary>
        /// Вычесление общей суммы системы (материалы и работы)
        /// </summary>
        public ICommand CalculateGeneralPrice { get; }

        private bool CanCalculateGeneralPrice(object p)
        {
            return Products.Count != 0;
        }

        private void OnCalculateGeneralPrice(object p)
        {
            try
            {
                ProductEntrySumm = (decimal)Products.Sum(pp => pp.Entry_Summ);
                ProductOutSumm = (decimal)Products.Sum(pp => pp.Out_Summ);
                ProductProceeds = ProductOutSumm - ProductEntrySumm;

                if (ProductOutSumm != 0)
                {
                    ProductProfit = RoundDecimal((ProductOutSumm - ProductEntrySumm) / ProductOutSumm * 100);
                }
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
        }

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

        #region МЕТОДЫ
        /// <summary>
        /// Обновление коллекции 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="oldItem"></param>
        /// <returns></returns>
        private T UpdateCollection<T>(ObservableCollection<T> collection, T oldItem)
        {
            if (collection == null)
            {
                return oldItem;
            }

            T newItem = oldItem;

            int indexNewItem = collection.IndexOf(oldItem);

            if (collection.Remove(oldItem))
            {
                collection.Insert(indexNewItem, newItem);
            }

            return newItem;
        }
        /// <summary>
        /// Округление цены
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private decimal RoundDecimal(decimal? number)
        {
            try
            {
                return decimal.Round((decimal)number, 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
                return (decimal)number;
            }
        }
        /// <summary>
        /// Обновление в коллекции индекса сортировки
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        private void UpdateSortOrder<T>(ObservableCollection<T> collection)
        {
            foreach (var item in collection)
            {
                int index = collection.IndexOf(item);

                if (item is ProductPart product)
                    product.Sort_Order = index;

                if (item is InstallPart install)
                    install.Sort_Order = index;
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
            Installs = new ObservableCollection<InstallPart>();

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);
            CalculatePrices = new LambdaCommand(OnCalculatePrices, CanCalculatePrices);
            CalculateGeneralPrice = new LambdaCommand(OnCalculateGeneralPrice, CanCalculateGeneralPrice);
            SwappingElementProduct = new LambdaCommand(OnSwappingElementProduct, CanSwappingElementProduct);
            SwappingElementInstall = new LambdaCommand(OnSwappingElementInstall, CanSwappingElementInstall);
            RemoveProduct = new LambdaCommand(OnRemoveProduct, CanRemoveProduct);
            RemoveInstall = new LambdaCommand(OnRemoveInstall, CanRemoveInstall);
        }
        #endregion
    }
}
