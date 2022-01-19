﻿using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class OfferManagerViewModel : ViewModel
    {
        #region ПОЛЯ
        private static readonly string _title = " :: Редактирование КП";

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

        #region репозитории
        /// <summary>
        /// Репозитории пользователей
        /// </summary>
        private readonly IRepository<Employees> RepositoryUsers;
        /// <summary>
        /// Репозитоторий КП
        /// </summary>
        private readonly IRepository<Offers> RepositoryOffer;
        /// <summary>
        /// Репозитоторий Систем
        /// </summary>
        private readonly IRepository<Parts> RepositoryPart;
        /// <summary>
        /// Репозиторий товаров
        /// </summary>
        private readonly IRepository<Products> RepositoryProduct;
        /// <summary>
        /// Репозиторий услуг
        /// </summary>
        private readonly IRepository<Installs> RepositoryInstall;
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

        private Offers _CurrentOffer;
        /// <summary>
        /// Теккущее КП
        /// </summary>
        public Offers CurrentOffer
        {
            get => _CurrentOffer;
            set => Set(ref _CurrentOffer, value);
        }

        private ObservableCollection<PartManagerViewModel> _Parts;
        /// <summary>
        /// Коллекция систем КП для TabItem
        /// </summary>
        public ObservableCollection<PartManagerViewModel> Parts
        {
            get => _Parts;
            set => Set(ref _Parts, value);
        }

        private PartManagerViewModel _SelectedPart;
        /// <summary>
        /// Выбранная система
        /// </summary>
        public PartManagerViewModel SelectedPart
        {
            get => _SelectedPart;
            set => Set(ref _SelectedPart, value);
        }

        private List<Products> _Products;
        /// <summary>
        /// Товары выбранного раздела КП
        /// </summary>
        public List<Products> Products
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

        private List<Installs> _Installs;
        /// <summary>
        /// Услуги выбранного раздела КП
        /// </summary>
        public List<Installs> Installs
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
        #endregion

        #region КОМАНДЫ

        #region загрузка данных из репозиториев
        /// <summary>
        /// Загрузка данных из репозиториев
        /// </summary>
        public ICommand LoadDataFromRepositories { get; }

        private bool CanLoadDataFromRepositories(object p)
        {
            return RepositoryUsers != null && RepositoryOffer != null;
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

                CurrentOffer = await RepositoryOffer.GetAsync(App.Host.Services.GetRequiredService<Offers>().Id);

                if (CurrentOffer == null) return;

                Products = await RepositoryProduct.Items
                    .Where(prod => prod.Categories.Sections
                    .Any(sec => sec.Id == CurrentOffer.Sections.Id))
                    .ToListAsync();

                Installs = await RepositoryInstall.Items
                    .Where(prod => prod.Categories.Sections
                    .Any(sec => sec.Id == CurrentOffer.Sections.Id))
                    .ToListAsync();

                foreach (Parts item in CurrentOffer.Parts)
                {
                    var partManagerView = App.Host.Services.GetRequiredService<PartManagerViewModel>();

                    partManagerView.Id = item.Id;

                    partManagerView.Name = item.Name;

                    if (partManagerView.LoadDataFromRepositories.CanExecute(item.Id))
                    {
                        partManagerView.LoadDataFromRepositories.Execute(item.Id);

                        Parts.Add(partManagerView);
                    }
                }
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

        #region добавление удаление данных

        /// <summary>
        /// Добавление системы в КП
        /// </summary>
        public ICommand AddNewPart { get; }

        private bool CanAddNewPart(object p) => CurrentOffer != null;

        private async void OnAddNewPart(object p)
        {
            Parts new_part = new Parts()
            {
                Name = "Система",
                Offer_Id = CurrentOffer.Id
            };

            try
            {
                new_part = await RepositoryPart.AddAsync(new_part);

                CurrentOffer.Parts.Add(new_part);

                var partManagerView = App.Host.Services.GetRequiredService<PartManagerViewModel>();

                partManagerView.Id = new_part.Id;

                partManagerView.Name = new_part.Name;

                Parts.Add(partManagerView);

                SelectedPart = partManagerView;
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
        }

        /// <summary>
        /// Удаление системы из КП
        /// </summary>
        public ICommand RemovePart { get; }

        private bool CanRemovePart(object p) => CurrentOffer != null && SelectedPart != null;

        private void OnRemovePart(object p)
        {
            Parts part_to_remove = CurrentOffer.Parts.FirstOrDefault(part => part.Id.Equals(SelectedPart.Id));

            if (part_to_remove == null) return;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить систему {part_to_remove.Name}?", "Удаление системы"))
            {
                return;
            }

            try
            {
                RepositoryPart.Remove(part_to_remove.Id);

                CurrentOffer.Parts.Remove(part_to_remove);

                Parts.Remove(SelectedPart);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
        }

        /// <summary>
        /// Сохранение/обновление текущего КП
        /// </summary>
        public ICommand UpdateOffer { get; }

        private bool CanUpdateOffer(object p)
        {
            return RepositoryOffer != null && CurrentOffer != null;
        }

        private async void OnUpdateOffer(object p)
        {
            Progress = true;
            try
            {
                foreach (var item in Parts)
                {
                    CurrentOffer.Parts.FirstOrDefault(part => part.Id == item.Id).Name = item.Name;
                }
                await RepositoryOffer.UpdateAsync(CurrentOffer);
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
        /// <summary>
        /// Добавление товара в систему
        /// </summary>
        public ICommand AddProduct { get; }

        private bool CanAddProduct(object p)
        {
            return SelectedPart != null && SelectedProduct != null;
        }

        private async void OnAddProduct(object p)
        {
            ProductPart productPart = SelectedProduct.ProductPart.FirstOrDefault(pp => pp.Part_Id.Equals(SelectedPart.Id));

            if (SelectedPart.Products.Contains(productPart))
            {
                UserDialog.ShowInformation("В данной системе уже есть такой товар", "Информация");

                return;
            }

            ProductPart new_productPart = new ProductPart()
            {
                Part_Id = SelectedPart.Id,
                Entry_Price = SelectedProduct.Entry_Price,
                Sort_Order = SelectedPart.Products.Count
            };

            SelectedProduct.ProductPart.Add(new_productPart);

            await RepositoryProduct.UpdateAsync(SelectedProduct);

            SelectedPart.Products.Add(new_productPart);
        }
        /// <summary>
        /// Добавление услуги в систему
        /// </summary>
        public ICommand AddInstall { get; }

        private bool CanAddInstall(object p)
        {
            return SelectedPart != null && SelectedInstall != null;
        }

        private async void OnAddInstall(object p)
        {
            InstallPart installPart = SelectedInstall.InstallPart.FirstOrDefault(ip => ip.Part_Id.Equals(SelectedPart.Id));

            if (SelectedPart.Installs.Contains(installPart))
            {
                UserDialog.ShowInformation("В данной системе уже есть такая услуга", "Информация");

                return;
            }

            InstallPart new_installPart = new InstallPart()
            {
                Part_Id = SelectedPart.Id,
                Entry_Price = SelectedInstall.Entry_Price,
                Sort_Order = SelectedPart.Installs.Count,
                Amount = 0
            };

            /*проверка правильности перебора для формирования кол-ва услуг*/
            foreach (var item in SelectedPart.Products)
            {
                if (item.Products.Category_Id.Equals(SelectedInstall.Category_Id) && SelectedInstall.Name.Contains(item.Products.Model))
                {
                    new_installPart.Amount += item.Amount;
                }
            }

            SelectedInstall.InstallPart.Add(new_installPart);

            await RepositoryInstall.UpdateAsync(SelectedInstall);

            SelectedPart.Installs.Add(new_installPart);
        }
        #endregion

        #region калькуляция цен
        /// <summary>
        /// Пересчет цен в КП при изменении коэф. наценок
        /// </summary>
        public ICommand CalculateAllPrice { get; }

        private bool CanCalculateAllPrice(object p)
        {
            return CurrentOffer != null && CurrentOffer.Parts != null && CurrentOffer.Parts.Count != 0;
        }

        private void OnCalculateAllPrice(object p)
        {
            foreach (Parts part in CurrentOffer.Parts)
            {
                foreach (ProductPart item in part.ProductPart)
                {
                    if (item.Out_Price != null)
                    {
                        item.Out_Price = RoundDecimal(item.Entry_Price * CurrentOffer.Configs.Margin_Product);
                    }
                    if (item.Entry_Summ != null)
                    {
                        item.Entry_Summ = RoundDecimal(item.Amount * item.Entry_Price);
                    }
                    if (item.Out_Summ != null)
                    {
                        item.Out_Summ = RoundDecimal(item.Amount * item.Out_Price);
                    }
                    //реализовать PropertyChanged
                    var partView = Parts.First(it => it.Id == item.Part_Id);

                    var prPart = partView.Products.First(it => it.Part_Id == item.Part_Id);
                   

                    prPart.Out_Price = item.Out_Price;
                    prPart.Out_Summ = item.Out_Summ;

                    partView.CalculateGeneralPriceProduct.Execute(null);
                    
                }
                foreach (InstallPart item in part.InstallPart)
                {
                    if (item.Out_Price != null)
                    {
                        item.Out_Price = RoundDecimal(item.Entry_Price * CurrentOffer.Configs.Margin_Work);
                    }
                    if (item.Entry_Summ != null)
                    {
                        item.Entry_Summ = RoundDecimal(item.Amount * item.Entry_Price);
                    }
                    if (item.Out_Summ != null)
                    {
                        item.Out_Summ = RoundDecimal(item.Amount * item.Out_Price);
                    }
                    //реализовать PropertyChanged
                    var partView = Parts.First(it => it.Id == item.Part_Id);

                    var insPart = partView.Installs.First(it => it.Part_Id == item.Part_Id);

                    insPart.Out_Price = item.Out_Price;
                    insPart.Out_Summ = item.Out_Summ;

                    partView.CalculateGeneralPriceInstall.Execute(null);
                    
                }
            }
            //Этот перебор не нужен
            /*
            foreach (PartManagerViewModel item in Parts)
            {
                if (item.Products.Count != 0)
                {
                    item.Products.Clear();
                }

                if (item.Installs.Count != 0)
                {
                    item.Installs.Clear();
                }

                if (item.LoadDataFromRepositories.CanExecute(item.Id))
                {
                    item.LoadDataFromRepositories.Execute(item.Id);
                }
            }
            */
            if (UpdateOffer.CanExecute(null))
            {
                UpdateOffer.Execute(null);
            }
        }
        #endregion

        #endregion

        #region МЕТОДЫ
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
        /// <summary>
        /// Правила огругления цен
        /// </summary>
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
        #endregion

        #region КОНСТРУКТОРЫ
        public OfferManagerViewModel(
           IRepository<Offers> repaOffer,
           IRepository<Employees> repaEmployee,
           IRepository<Parts> repaPart,
           IRepository<Products> repaProduct,
           IRepository<Installs> repaInstall,
           IUserDialog userDialog)
        {
            Progress = true;

            Parts = new ObservableCollection<PartManagerViewModel>();
            Products = new List<Products>();
            Installs = new List<Installs>();

            RepositoryUsers = repaEmployee;
            RepositoryOffer = repaOffer;
            RepositoryPart = repaPart;
            RepositoryProduct = repaProduct;
            RepositoryInstall = repaInstall;

            UserDialog = userDialog;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);

            AddNewPart = new LambdaCommand(OnAddNewPart, CanAddNewPart);
            RemovePart = new LambdaCommand(OnRemovePart, CanRemovePart);

            UpdateOffer = new LambdaCommand(OnUpdateOffer, CanUpdateOffer);

            AddProduct = new LambdaCommand(OnAddProduct, CanAddProduct);
            AddInstall = new LambdaCommand(OnAddInstall, CanAddInstall);

            CalculateAllPrice = new LambdaCommand(OnCalculateAllPrice, CanCalculateAllPrice);
        }
        #endregion
    }
}
