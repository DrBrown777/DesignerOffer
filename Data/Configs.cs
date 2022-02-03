namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Configs : IEntity, ICloneable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public decimal Margin_Product { get; set; }

        public decimal Margin_Work { get; set; }

        public decimal Margin_Admin { get; set; }

        public virtual Offers Offers { get; set; }

        public object Clone()
        {
            return new Configs()
            {
                Margin_Product = Margin_Product,
                Margin_Work = Margin_Work,
                Margin_Admin = Margin_Admin
            };
        }
    }
}
