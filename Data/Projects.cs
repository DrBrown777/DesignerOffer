using Designer_Offer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Designer_Offer.Data
{
    public partial class Projects : IEntity
    {
        public Projects()
        {
            Offers = new HashSet<Offers>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? Employee_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public virtual Builds Builds { get; set; }

        public virtual Employees Employees { get; set; }

        public virtual ICollection<Offers> Offers { get; set; }
    }
}
