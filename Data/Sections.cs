using Designer_Offer.Models.Base;
using Designer_Offer.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Sections : Model, IEntity
    {
        public Sections()
        {
            Offers = new HashSet<Offers>();
            Categories = new HashSet<Categories>();
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

        public virtual ICollection<Offers> Offers { get; set; }

        public virtual ICollection<Categories> Categories { get; set; }
    }
}
