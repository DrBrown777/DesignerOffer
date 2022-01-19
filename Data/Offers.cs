namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Offers : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Offers()
        {
            Parts = new HashSet<Parts>();
        }

        public int Id { get; set; }

        public int? Section_Id { get; set; }

        public int Project_Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public virtual Configs Configs { get; set; }

        public virtual Projects Projects { get; set; }

        public virtual Sections Sections { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Parts> Parts { get; set; }
    }
}
