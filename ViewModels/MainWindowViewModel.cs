using Designer_Offer.ViewModels.Base;
using Designer_Offer.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private static Login _LoginPage = null;
        /// <summary>
        /// Страница Логина
        /// </summary>
        public Login LoginPage
        {
            get => _LoginPage;
            set => Set(ref _LoginPage, value);
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

        public MainWindowViewModel()
        {
            _LoginPage = new Login();
        }
    }
}
