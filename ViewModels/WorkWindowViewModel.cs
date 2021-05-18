using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
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
        private ICommand ShowProjectManager { get; }

        private void OnShowProjectManagerCommand(object p)
        {
            
        }

        private bool CanShowProjectManagerCommand(object p)
        {
            return true;
        }

        #endregion

        public WorkWindowViewModel()
        {
            ShowProjectManager = new LambdaCommand(OnShowProjectManagerCommand, CanShowProjectManagerCommand);
        }
    }
}
