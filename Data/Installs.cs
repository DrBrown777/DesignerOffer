using Designer_Offer.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Installs : IEntity
    {
        public Installs()
        {
            InstallPart = new HashSet<InstallPart>();
        }

        public int Id { get; set; }

        public int? Category_Id { get; set; }

        public int? Unit_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public decimal? Entry_Price { get; set; }

        public virtual Categories Categories { get; set; }

        public virtual ICollection<InstallPart> InstallPart { get; set; }

        public virtual Units Units { get; set; }
    }
}
