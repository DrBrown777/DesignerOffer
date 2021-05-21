using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    class WorkWindowViewModel : ViewModel
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

        private bool CanShowProjectManagerCommand(object p) => true && CurrentModel == null;
        #endregion

        #region КОНСТРУКТОРЫ

        public WorkWindowViewModel()
        {
            ShowProjectManager = new LambdaCommand(OnShowProjectManagerCommand, CanShowProjectManagerCommand);
        }
        #endregion
    }
}
