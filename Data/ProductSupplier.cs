namespace Designer_Offer.Data
{
    using Designer_Offer.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductSupplier")]
    public partial class ProductSupplier : IEntity
    {
        public int Id { get; set; }

        public int Product_Id { get; set; }

        public int Supplier_Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
