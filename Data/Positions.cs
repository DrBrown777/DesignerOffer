using Designer_Offer.Models.Base;
using Designer_Offer.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Positions : Model, IEntity
    {
        public Positions()
        {
            Employees = new HashSet<Employees>();
            Companies = new HashSet<Companies>();
        }

        public int Id { get; set; }

        private string _Name;
        [Required]
        [StringLength(50)]
        public string Name 
        { 
            get => _Name; 
            set => Set(ref _Name, value); 
        }

        public virtual ICollection<Employees> Employees { get; set; }

        public virtual ICollection<Companies> Companies { get; set; }
    }
}
