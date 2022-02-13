using Designer_Offer.Models.Base;
using Designer_Offer.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Units : Model, IEntity
    {
        public Units()
        {
            Installs = new HashSet<Installs>();
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }

        private string _Name;
        [Required]
        [StringLength(20)]
        public string Name { get => _Name; set => Set(ref _Name, value); }

        public virtual ICollection<Installs> Installs { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
