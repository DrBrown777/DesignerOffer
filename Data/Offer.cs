namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Offer")]
    public partial class Offer : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Offer()
        {
            Part = new HashSet<Part>();
        }

        public int Id { get; set; }

        public int? Section_Id { get; set; }

        public int Project_Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public virtual Config Config { get; set; }

        public virtual Project Project { get; set; }

        public virtual Section Section { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Part> Part { get; set; }
    }
}
