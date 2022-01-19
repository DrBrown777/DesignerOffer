namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Installs : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstallPart> InstallPart { get; set; }

        public virtual Units Units { get; set; }
    }
}
