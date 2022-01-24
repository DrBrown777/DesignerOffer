namespace Designer_Offer.Data
{
    using Designer_Offer.Models.Base;
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employees : Model, IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employees()
        {
            Projects = new HashSet<Projects>();
        }

        public int Id { get; set; }

        public int? Company_Id { get; set; }

        public int? Position_Id { get; set; }

        private string _First_Name;
        [Required]
        [StringLength(50)]
        public string First_Name 
        { 
            get => _First_Name; 
            set => Set(ref _First_Name, value); 
        }

        private string _Last_Name;
        [Required]
        [StringLength(50)]
        public string Last_Name 
        { 
            get => _Last_Name; 
            set => Set(ref _Last_Name, value); 
        }

        private string _Phone;
        [Required]
        [StringLength(20)]
        public string Phone 
        { 
            get => _Phone; 
            set => Set(ref _Phone, value); 
        }

        private string _Mail;
        [Required]
        [StringLength(30)]
        public string Mail 
        { 
            get => _Mail; 
            set => Set(ref _Mail, value); 
        }

        public virtual Companies Companies { get; set; }

        public virtual Positions Positions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Projects> Projects { get; set; }

        public virtual UsersData UsersData { get; set; }
    }
}
