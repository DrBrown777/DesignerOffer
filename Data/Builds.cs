namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Builds : IEntity
    {
        public int Id { get; set; }

        public int? Client_Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Adress { get; set; }

        public virtual Clients Clients { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
