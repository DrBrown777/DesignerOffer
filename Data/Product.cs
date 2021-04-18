namespace Designer_Offer.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            ProductSupplier = new HashSet<ProductSupplier>();
            ProductPart = new HashSet<ProductPart>();
        }

        public int Id { get; set; }

        public int? Category_Id { get; set; }

        public int? Unit_id { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; }

        public decimal? Entry_Price { get; set; }

        public virtual Category Category { get; set; }

        public virtual Unit Unit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductSupplier> ProductSupplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPart> ProductPart { get; set; }
    }
}