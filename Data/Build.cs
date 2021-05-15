namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Build")]
    public partial class Build : IEntity
    {
        public int Id { get; set; }

        public int? Project_Id { get; set; }

        public int Client_Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Adress { get; set; }

        public virtual Client Client { get; set; }

        public virtual Project Project { get; set; }
    }
}
