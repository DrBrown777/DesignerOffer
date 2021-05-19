using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    class WorkWindowViewModel : ViewModel
    {
        #region СВОЙСТВА
        private string _Title = "Управление проектами";
        /// <summary>
        /// Заголовок Окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private string _Status = "Готов";
        /// <summary>
        /// Статус программы
        /// </summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

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

        private bool CanShowProjectManagerCommand(object p) => true;
        #endregion

        public WorkWindowViewModel()
        {
            ShowProjectManager = new LambdaCommand(OnShowProjectManagerCommand, CanShowProjectManagerCommand);
        }
    }
}
