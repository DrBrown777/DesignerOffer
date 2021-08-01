using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class WorkWindowViewModel : ViewModel
    {
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
        #endregion

        #region КОНСТРУКТОРЫ
        public WorkWindowViewModel()
        {
            ShowProjectManager = new LambdaCommand(OnShowProjectManagerCommand, CanShowProjectManagerCommand);
            ShowCompanyManager = new LambdaCommand(OnShowCompanyManagerCommand, CanShowCompanyManagerCommand);
            ShowServiceManager = new LambdaCommand(OnShowServiceManagerCommand, CanShowServiceManagerCommand);
        }
        #endregion
    }
}
