namespace Designer_Offer.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Config")]
    public partial class Config
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Offer_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal Margin_Work { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal Margin_Product { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal Margin_Admin { get; set; }

        public virtual Offer Offer { get; set; }
    }
}
