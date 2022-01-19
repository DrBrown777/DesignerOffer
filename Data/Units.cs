namespace Designer_Offer.Data
{
    using Designer_Offer.Models.Base;
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Units : Model, IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Units()
        {
            Installs = new HashSet<Installs>();
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }

        private string _Name;
        [Required]
        [StringLength(20)]
        public string Name { get => _Name; set => Set(ref _Name, value); }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Installs> Installs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }
    }
}
