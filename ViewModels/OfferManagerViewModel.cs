using Designer_Offer.Data;
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

                foreach (Part item in CurrentOffer.Part)
                {
                    var partManagerView = App.Host.Services.GetRequiredService<PartManagerViewModel>();

                    partManagerView.Name = item.Name;

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

        #endregion

        #region КОНСТРУКТОРЫ
        public OfferManagerViewModel(
           IRepository<Offer> repaOffer,
           IRepository<Employee> repaEmployee,
           IUserDialog userDialog)
        {
            Progress = true;

            Parts = new ObservableCollection<PartManagerViewModel>();

            RepositoryUsers = repaEmployee;
            RepositoryOffer = repaOffer;

            UserDialog = userDialog;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);
        }
        #endregion
    }
}
