namespace Designer_Offer.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductSupplier")]
    public partial class ProductSupplier
    {
        public int Id { get; set; }

        public int Product_Id { get; set; }

        public int Supplier_Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
