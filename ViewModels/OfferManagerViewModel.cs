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
        /// <summary>
        /// Репозитоторий Систем
        /// </summary>
        private readonly IRepository<Part> RepositoryPart;
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

                if (CurrentOffer == null) return;

                foreach (Part item in CurrentOffer.Part)
                {
                    var partManagerView = App.Host.Services.GetRequiredService<PartManagerViewModel>();

                    partManagerView.Id = item.Id;

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
        #endregion
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

        #endregion

        #region КОНСТРУКТОРЫ
        public OfferManagerViewModel(
           IRepository<Offer> repaOffer,
           IRepository<Employee> repaEmployee,
           IRepository<Part> repaPart,
           IUserDialog userDialog)
        {
            Progress = true;

            Parts = new ObservableCollection<PartManagerViewModel>();

            RepositoryUsers = repaEmployee;
            RepositoryOffer = repaOffer;
            RepositoryPart = repaPart;

            UserDialog = userDialog;

            LoadDataFromRepositories = new LambdaCommand(OnLoadDataFromRepositories, CanLoadDataFromRepositories);

            AddNewPart = new LambdaCommand(OnAddNewPart, CanAddNewPart);
            RemovePart = new LambdaCommand(OnRemovePart, CanRemovePart);
            
            UpdateOffer = new LambdaCommand(OnUpdateOffer, CanUpdateOffer);
        }
        #endregion
    }
}
