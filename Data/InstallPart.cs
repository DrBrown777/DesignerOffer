namespace Designer_Offer.Data
{
    using Designer_Offer.Models.Base;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InstallPart")]
    public partial class InstallPart : Model
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Part_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Install_Id { get; set; }

        public decimal? Entry_Price { get; set; }

        public decimal? Entry_Summ { get; set; }
        
        private decimal? _Out_Price;
        public decimal? Out_Price { get => _Out_Price; set => Set(ref _Out_Price, value); }

        private decimal? _Out_Summ;
        public decimal? Out_Summ { get => _Out_Summ; set => Set(ref _Out_Summ, value); }

        public decimal? Amount { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public int? Sort_Order { get; set; }

        public virtual Install Install { get; set; }

        public virtual Part Part { get; set; }
    }
}
