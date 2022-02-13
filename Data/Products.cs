using Designer_Offer.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Products : IEntity
    {
        public Products()
        {
            ProductPart = new HashSet<ProductPart>();
            Suppliers = new HashSet<Suppliers>();
        }

        public int Id { get; set; }

        public int? Category_Id { get; set; }

        public int? Unit_id { get; set; }

        public int? Manufacturer_Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Model { get; set; }

        public decimal? Entry_Price { get; set; }

        public virtual Categories Categories { get; set; }

        public virtual Manufacturers Manufacturers { get; set; }

        public virtual ICollection<ProductPart> ProductPart { get; set; }

        public virtual Units Units { get; set; }

        public virtual ICollection<Suppliers> Suppliers { get; set; }
    }
}
