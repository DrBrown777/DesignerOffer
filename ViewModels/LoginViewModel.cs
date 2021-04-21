using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class LoginViewModel : ViewModel
    {
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

        private List<Company> _Companies;
        /// <summary>
        /// Список компаний
        /// </summary>
        public List<Company> Companies
        {
            get => _Companies;
            set => Set(ref _Companies, value);
        }

        private string _Login;
        /// <summary>
        /// Поле Логин
        /// </summary>
        public string Login
        {
            get => _Login;
            set => Set(ref _Login, value);
        }


        //public static Func<string> PasswordHandler { get; set; }

        public ICommand Command { get; }

        private void OnCommandExecuted(object p)
        {
            
        }

        private bool CanOnCommandExecute(object p) => true;

        public ICommand DisplayLoginView
        {
            get
            {
                return new LambdaCommand(action => MainWindowViewModel.ViewModel = new LoginViewModel(),
                canExecute => true);
            }
        }

        public LoginViewModel()
        {
            Companies = contextDB.Company.ToList();
            Command = new LambdaCommand(OnCommandExecuted, CanOnCommandExecute);
        }

    }
}
