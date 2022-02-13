using Designer_Offer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Parts : IEntity, ICloneable
    {
        public Parts()
        {
            InstallPart = new HashSet<InstallPart>();
            ProductPart = new HashSet<ProductPart>();
        }

        public int Id { get; set; }

        public int Offer_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<InstallPart> InstallPart { get; set; }

        public virtual Offers Offers { get; set; }

        public virtual ICollection<ProductPart> ProductPart { get; set; }

        public object Clone()
        {
            Parts part = new Parts()
            {
                Name = Name,
                ProductPart = new HashSet<ProductPart>(),
                InstallPart = new HashSet<InstallPart>()
            };

            foreach (var item in ProductPart)
            {
                part.ProductPart.Add((ProductPart)item.Clone());
            }

            foreach (var item in InstallPart)
            {
                part.InstallPart.Add((InstallPart)item.Clone());
            }

            return part;
        }
    }
}
