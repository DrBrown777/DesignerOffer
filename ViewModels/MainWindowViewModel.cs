using Designer_Offer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer_Offer.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Заголовок окна
        private string _Title = "Designer Offer";

        /// <summary>
        /// Заголовок Окна
        /// </summary>
        public string Title
        {
            get => _Title;

            set => Set(ref _Title, value);
        }
        #endregion
    }
}
