namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CompanyPosition")]
    public partial class CompanyPosition : IEntity
    {
        public int Id { get; set; }

        public int Company_Id { get; set; }

        public int Position_Id { get; set; }

        public virtual Company Company { get; set; }

        public virtual Position Position { get; set; }
    }
}
