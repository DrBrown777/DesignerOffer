using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class CompanyEditorViewModel : ViewModel
    {
        private readonly IRepository<Position> RepositoryPosition;

        private string _Name;
        /// <summary>
        /// Название компании
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private string _Address;
        /// <summary>
        /// Адресс компании
        /// </summary>
        public string Address
        {
            get => _Address;
            set => Set(ref _Address, value); 
        }

        private string _Phone;
        /// <summary>
        /// Телефон компании
        /// </summary>
        public string Phone
        {
            get => _Phone;
            set => Set(ref _Phone, value);
        }

        private string _Email;
        /// <summary>
        /// E-mail компании
        /// </summary>
        public string Email
        {
            get => _Email;
            set => Set(ref _Email, value);
        }

        private ICollection<Position> _CompanyPosition;
        /// <summary>
        /// Должности компании
        /// </summary>
        public ICollection<Position> CompanyPosition
        {
            get => _CompanyPosition;
            set => Set(ref _CompanyPosition, value);
        }

        private List<Position> _Position;
        /// <summary>
        /// Все должности
        /// </summary>
        public List<Position> Position
        {
            get => _Position;
            set => Set(ref _Position, value);
        }

        public CompanyEditorViewModel(IRepository<Position> repaposition)
        {
            RepositoryPosition = repaposition;
            //Position = CompanyPosition.ToList();
        }
    }
}
