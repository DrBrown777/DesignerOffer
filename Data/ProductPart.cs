namespace Designer_Offer.Data
{
    using Designer_Offer.Models.Base;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductPart")]
    public partial class ProductPart : Model
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Part_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Product_Id { get; set; }

        public decimal? Entry_Price { get; set; }

        private decimal? _Entry_Summ;
        public decimal? Entry_Summ
        {
            get => _Entry_Summ;
            set => Set(ref _Entry_Summ, value);
        }

        private decimal? _Out_Price;
        public decimal? Out_Price 
        { 
            get => _Out_Price; 
            set => Set(ref _Out_Price, value); 
        }

        private decimal? _Out_Summ;
        public decimal? Out_Summ 
        { 
            get => _Out_Summ; 
            set => Set(ref _Out_Summ, value); 
        }

        public decimal? Amount { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public int? Sort_Order { get; set; }

        public virtual Parts Parts { get; set; }

        public virtual Products Products { get; set; }
    }
}
