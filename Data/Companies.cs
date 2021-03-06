using Designer_Offer.Models.Base;
using Designer_Offer.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Companies : Model, IEntity
    {
        public Companies()
        {
            Employees = new HashSet<Employees>();
            Positions = new HashSet<Positions>();
        }

        public int Id { get; set; }

        private string _Name;
        [Required]
        [StringLength(100)]
        public string Name { get => _Name; set => Set(ref _Name, value); }

        private string _Adress;
        [Required]
        [StringLength(200)]
        public string Adress { get => _Adress; set => Set(ref _Adress, value); }

        private string _Phone;
        [Required]
        [StringLength(20)]
        public string Phone { get => _Phone; set => Set(ref _Phone, value); }

        private string _Mail;
        [Required]
        [StringLength(30)]
        public string Mail { get => _Mail; set => Set(ref _Mail, value); }

        public virtual ICollection<Employees> Employees { get; set; }

        public virtual ICollection<Positions> Positions { get; set; }
    }
}
