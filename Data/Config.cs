namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Config")]
    public partial class Config : IEntity
    {
        public int Id { get; set; }

        public int Offer_Id { get; set; }

        public decimal Margin_Product { get; set; }

        public decimal Margin_Work { get; set; }

        public decimal Margin_Admin { get; set; }

        public virtual Offer Offer { get; set; }
    }
}
