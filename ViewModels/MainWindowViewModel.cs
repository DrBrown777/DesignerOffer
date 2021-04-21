using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using Designer_Offer.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        //private static ViewModel _ViewModel;

        public static ViewModel ViewModel 
        {
            get; set;
        }

        private string _Title = "Designer Offer";
        /// <summary>
        /// Заголовок Окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        private string _Status = "Для входа в систему введите Логин и Пароль";
        /// <summary>
        /// Статус программы
        /// </summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

        //private Page _Page;
        ///// <summary>
        ///// Любая страница для Frame
        ///// </summary>
        //public Page Page
        //{
        //    get => _Page;
        //    set => Set(ref _Page, value);
        //}

        private static Login _LoginPage = null;
        /// <summary>
        /// Страница Логина
        /// </summary>
        public Login LoginPage
        {
            get => _LoginPage;
            set => Set(ref _LoginPage, value);
        }

        public ICommand DisplayLoginView
        {
            get
            {
                return new LambdaCommand(action => ViewModel = new LoginViewModel(),
                canExecute => true);
            }
        }

        public MainWindowViewModel()
        {
            //LoginPage = new Login();
            //Page = LoginPage;
            ViewModel = new LoginViewModel();
        }
    }
}
