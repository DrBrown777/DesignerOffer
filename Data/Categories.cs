using Designer_Offer.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Categories : IEntity
    {
        public Categories()
        {
            Installs = new HashSet<Installs>();
            Products = new HashSet<Products>();
            Sections = new HashSet<Sections>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Installs> Installs { get; set; }

        public virtual ICollection<Products> Products { get; set; }

        public virtual ICollection<Sections> Sections { get; set; }
    }
}
