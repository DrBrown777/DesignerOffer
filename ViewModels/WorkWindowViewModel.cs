using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class WorkWindowViewModel : ViewModel
    {
        #region ПОЛЯ
        /// <summary>
        /// Сервис экспорта в файл
        /// </summary>
        private readonly IExportService ExportService;

        /// <summary>
        /// Сервис Диалогов
        /// </summary>
        private readonly IUserDialog UserDialog;
        #endregion

        #region СВОЙСТВА
        private ViewModel _CurrentModel;
        /// <summary>
        /// Текущая дочерняя модель представления
        /// </summary>
        public ViewModel CurrentModel
        {
            get => _CurrentModel;
            set => Set(ref _CurrentModel, value);
        }
        #endregion

        #region КОМАНДЫ
        /// <summary>
        /// Команда отображения страницы Менеджера Проектов
        /// </summary>
        public ICommand ShowProjectManager { get; }

        private void OnShowProjectManagerCommand(object p)
        {
            CurrentModel = App.Host.Services.GetRequiredService<ProjectManagerViewModel>();
        }

        private bool CanShowProjectManagerCommand(object p)
        {
            if (!ReferenceEquals(CurrentModel, App.Host.Services.GetRequiredService<ProjectManagerViewModel>()))
                return true;

            return false;
        }
        /// <summary>
        /// Команда отображения страницы Менеджера Компаний
        /// </summary>
        public ICommand ShowCompanyManager { get; }

        private void OnShowCompanyManagerCommand(object p)
        {
            CurrentModel = App.Host.Services.GetRequiredService<CompanyManagerViewModel>();
        }

        private bool CanShowCompanyManagerCommand(object p)
        {
            if (!ReferenceEquals(CurrentModel, App.Host.Services.GetRequiredService<CompanyManagerViewModel>()))
                return true;
            
            return false;
        }
        /// <summary>
        /// Команда отображения страницы Менеджера услуг
        /// </summary>
        public ICommand ShowServiceManager { get; }

        private void OnShowServiceManagerCommand(object p)
        {
            CurrentModel = App.Host.Services.GetRequiredService<ServiceManagerViewModel>();
        }

        private bool CanShowServiceManagerCommand(object p)
        {
            if (!ReferenceEquals(CurrentModel, App.Host.Services.GetRequiredService<ServiceManagerViewModel>()))
                return true;

            return false;
        }
        /// <summary>
        /// Команда отображения Менеджера КП
        /// </summary>
        public ICommand ShowOfferManager { get; }

        private void OnShowOfferManagerCommand(object p)
        {
            if (p != null && p is Offers)
            {
                Offers offer = (Offers)p;
                App.Host.Services.GetRequiredService<Offers>().Id = offer.Id;
            }
                
            CurrentModel = App.Host.Services.GetRequiredService<OfferManagerViewModel>();
        }

        private bool CanShowOfferManagerCommand(object p)
        {
            return p is Offers && !ReferenceEquals(CurrentModel, App.Host.Services.GetRequiredService<OfferManagerViewModel>());
        }
        /// <summary>
        /// Команда экспорта в файл
        /// </summary>
        public ICommand ExportToExcel { get; }

        private bool CanExportToExcel(object p)
        {
            return App.Host.Services.GetRequiredService<ProjectManagerViewModel>().SelectedOffer != null &&
                ReferenceEquals(CurrentModel, App.Host.Services.GetRequiredService<ProjectManagerViewModel>());
        }

        private void OnExportToExcel(object p)
        {
            Offers offer_to_export = App.Host.Services.GetRequiredService<ProjectManagerViewModel>().SelectedOffer;

            if (!ExportService.ExportToExcel(offer_to_export))
            {
                return;
            }

            UserDialog.ShowInformation($"Экспорт КП {offer_to_export.Name} выполнен успешно!", "Информация");
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public WorkWindowViewModel(IExportService _exportService, IUserDialog _userDialog)
        {
            ExportService = _exportService;
            UserDialog = _userDialog;

            ShowProjectManager = new LambdaCommand(OnShowProjectManagerCommand, CanShowProjectManagerCommand);
            ShowCompanyManager = new LambdaCommand(OnShowCompanyManagerCommand, CanShowCompanyManagerCommand);
            ShowServiceManager = new LambdaCommand(OnShowServiceManagerCommand, CanShowServiceManagerCommand);
            ShowOfferManager = new LambdaCommand(OnShowOfferManagerCommand, CanShowOfferManagerCommand);

            ExportToExcel = new LambdaCommand(OnExportToExcel, CanExportToExcel);
        }
        #endregion
    }
}
