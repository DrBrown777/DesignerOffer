using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class EmployeeEditorViewModel : ViewModel
    {
        #region СВОЙСТВА
        private string _UserLogin;
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserLogin
        {
            get => _UserLogin;
            set => Set(ref _UserLogin, value);
        }

        private string _UserPassword;
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserPassword
        {
            get => _UserPassword;
            set => Set(ref _UserPassword, value);
        }

        private string _UserName;
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName
        {
            get => _UserName;
            set => Set(ref _UserName, value);
        }

        private string _UserSurName;
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string UserSurName
        {
            get => _UserSurName;
            set => Set(ref _UserSurName, value);
        }

        private string _UserEmail;
        /// <summary>
        /// E-mail пользователя
        /// </summary>
        public string UserEmail
        {
            get => _UserEmail;
            set => Set(ref _UserEmail, value);
        }

        private string _UserPhone;
        /// <summary>
        /// Телефон пользователя
        /// </summary>
        public string UserPhone
        {
            get => _UserPhone;
            set => Set(ref _UserPhone, value);
        }

        private List<Companies> _Companies;
        /// <summary>
        /// Список компаний
        /// </summary>
        public List<Companies> Companies
        {
            get => _Companies;
            set => Set(ref _Companies, value);
        }

        private List<Positions> _Positions;
        /// <summary>
        /// Список должностей выбранной компании
        /// </summary>
        public List<Positions> Positions
        {
            get => _Positions;
            set => Set(ref _Positions, value);
        }

        private Companies _SelectedComapany;
        /// <summary>
        /// Выбранная компания
        /// </summary>
        public Companies SelectedCompany
        {
            get => _SelectedComapany;
            set => Set(ref _SelectedComapany, value);
        }

        private Positions _SelectedPosition;
        /// <summary>
        /// Выбранная должность
        /// </summary>
        public Positions SelectedPosition
        {
            get => _SelectedPosition;
            set => Set(ref _SelectedPosition, value);
        }
        #endregion

        #region КОМАНДЫ
        /// <summary>
        /// Команда заполняет должности выбранной компании
        /// </summary>
        public ICommand LoadPositionCommand { get; }

        private void OnLoadPositionCommand(object p)
        {
            Positions = SelectedCompany.Positions.ToList();
        }

        private bool CanLoadPositionCommand(object p)
        {
            return true && SelectedCompany != null && SelectedCompany.Positions != null;
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public EmployeeEditorViewModel(
            IRepository<Companies> repaCompanies,
            IRepository<Positions> repaPositions)
        {
            Companies = repaCompanies.Items.ToList();
            Positions = repaPositions.Items.ToList();

            LoadPositionCommand = new LambdaCommand(OnLoadPositionCommand, CanLoadPositionCommand);
        }
        #endregion
    }
}
