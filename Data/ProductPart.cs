namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductPart")]
    public partial class ProductPart : IEntity
    {
        public int Id { get; set; }

        public int Product_Id { get; set; }

        public int Part_Id { get; set; }

        public decimal? Count_Product { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public virtual Part Part { get; set; }

        public virtual Product Product { get; set; }
    }
}
