namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Part")]
    public partial class Part : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Part()
        {
            InstallPart = new HashSet<InstallPart>();
            ProductPart = new HashSet<ProductPart>();
        }

        public int Id { get; set; }

        public int Offer_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public decimal? Entry_Cost { get; set; }

        public decimal? Out_Cost { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstallPart> InstallPart { get; set; }

        public virtual Offer Offer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPart> ProductPart { get; set; }
    }
}
