using Designer_Offer.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Suppliers : IEntity
    {
        public Suppliers()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
