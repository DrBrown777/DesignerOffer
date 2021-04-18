using Designer_Offer.ViewModels.Base;
using Designer_Offer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private string _Title = "Designer Offer";
        private string _Status = "Готов!";

        /// <summary>
        /// Заголовок Окна
        /// </summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }
    }
}
