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
        private readonly IRepository<Part> RepositoryPart;

        private readonly IUserDialog UserDialog;

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
        /// Коллекция 
        /// </summary>
        public ObservableCollection<ProductPart> Products
        {
            get => _Products;
            set => Set(ref _Products, value);
        }

        private ProductPart _SelectedProduct;
        /// <summary>
        /// 
        /// </summary>
        public ProductPart SelectedProduct
        {
            get => _SelectedProduct;
            set => Set(ref _SelectedProduct, value);
        }
        #endregion

        #region загрузка данных из репозиториев
        /// <summary>
        /// Загрузка данных из репозиториев
        /// </summary>
        public ICommand LoadDataFromRepositories { get; }

        private bool CanLoadDataFromRepositories(object p)
        {
            return true;
        }

        private void OnLoadDataFromRepositories(object p)
        {
            try
            {
                Products = new ObservableCollection<ProductPart>(RepositoryPart.Get(Id).ProductPart);
            }
            catch (Exception e)
            {
                UserDialog.ShowError(e.Message, "Ошибка");
            }
        }
        #endregion

        public PartManagerViewModel(IRepository<Part> repaPart, IUserDialog userDialog)
        {
            RepositoryPart = repaPart;
            UserDialog = userDialog;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);
        }
    }
}
