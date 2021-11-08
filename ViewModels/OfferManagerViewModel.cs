using Designer_Offer.Data;
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
        private Employee CurrentUser;

        /// <summary>
        /// Текущая компания
        /// </summary>
        private Company CurrentCompany;

        /// <summary>
        /// Сервис диалогов
        /// </summary>
        private readonly IUserDialog UserDialog;

        #region репозитории
        /// <summary>
        /// Репозитории пользователей
        /// </summary>
        private readonly IRepository<Employee> RepositoryUsers;
        /// <summary>
        /// Репозитоторий КП
        /// </summary>
        private readonly IRepository<Offer> RepositoryOffer;
        /// <summary>
        /// Репозитоторий Систем
        /// </summary>
        private readonly IRepository<Part> RepositoryPart;
        /// <summary>
        /// Репозиторий товаров
        /// </summary>
        private readonly IRepository<Product> RepositoryProduct;
        /// <summary>
        /// Репозиторий услуг
        /// </summary>
        private readonly IRepository<Install> RepositoryInstall;
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
        
        private Offer _CurrentOffer;
        /// <summary>
        /// Теккущее КП
        /// </summary>
        public Offer CurrentOffer
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

        private List<Product> _Products;
        /// <summary>
        /// Товары выбранного раздела КП
        /// </summary>
        public List<Product> Products
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

        private List<Install> _Installs;
        /// <summary>
        /// Услуги выбранного раздела КП
        /// </summary>
        public List<Install> Installs
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

                CurrentCompany = CurrentUser.Company;

                Title = CurrentCompany?.Name + _title;

                CurrentOffer = await RepositoryOffer.GetAsync(App.Host.Services.GetRequiredService<Offer>().Id);

                if (CurrentOffer == null) return;

                Products = await RepositoryProduct.Items
                    .Where(prod => prod.Category.Section
                    .Any(sec => sec.Id == CurrentOffer.Section.Id))
                    .ToListAsync();

                Installs = await RepositoryInstall.Items
                    .Where(prod => prod.Category.Section
                    .Any(sec => sec.Id == CurrentOffer.Section.Id))
                    .ToListAsync();

                foreach (Part item in CurrentOffer.Part)
                {
                    var partManagerView = App.Host.Services.GetRequiredService<PartManagerViewModel>();

                    partManagerView.Id = item.Id;

                    partManagerView.Name = item.Name;

                    partManagerView.LoadDataFromRepositories.Execute(item.Id);

                    Parts.Add(partManagerView);
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
            Part new_part = new Part()
            {
                Name = "Система",
                Offer_Id = CurrentOffer.Id
            };

            try
            {
                new_part = await RepositoryPart.AddAsync(new_part);

                CurrentOffer.Part.Add(new_part);

                var partManagerView = App.Host.Services.GetRequiredService<PartManagerViewModel>();

                partManagerView.Id = new_part.Id;

                partManagerView.Name = new_part.Name;

                Parts.Add(partManagerView);
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
            Part part_to_remove = CurrentOffer.Part.FirstOrDefault(part => part.Id.Equals(SelectedPart.Id));

            if (part_to_remove == null) return;

            if (!UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить систему {part_to_remove.Name}?", "Удаление системы"))
            {
                return;
            }

            try
            {
                RepositoryPart.Remove(part_to_remove.Id);

                CurrentOffer.Part.Remove(part_to_remove);

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
                    CurrentOffer.Part.FirstOrDefault(part => part.Id == item.Id).Name = item.Name;
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

        private void OnAddProduct(object p)
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
                Entry_Price = SelectedProduct.Entry_Price
            };

            SelectedProduct.ProductPart.Add(new_productPart);

            RepositoryProduct.Update(SelectedProduct);

            SelectedPart.Products.Add(new_productPart);
        }
        #endregion

        #endregion

        #region МЕТОДЫ
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
        public OfferManagerViewModel(
           IRepository<Offer> repaOffer,
           IRepository<Employee> repaEmployee,
           IRepository<Part> repaPart,
           IRepository<Product> repaProduct,
           IRepository<Install> repaInstall,
           IUserDialog userDialog)
        {
            Progress = true;

            Parts = new ObservableCollection<PartManagerViewModel>();
            Products = new List<Product>();
            Installs = new List<Install>();

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
        }
        #endregion
    }
}
