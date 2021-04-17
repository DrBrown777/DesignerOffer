namespace Designer_Offer.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Install")]
    public partial class Install
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Install()
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

        public virtual Category Category { get; set; }

        public virtual Unit Unit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstallPart> InstallPart { get; set; }
    }
}
