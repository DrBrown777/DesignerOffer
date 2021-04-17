namespace Designer_Offer.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InstallPart")]
    public partial class InstallPart
    {
        public int Id { get; set; }

        public int Install_Id { get; set; }

        public int Part_Id { get; set; }

        public decimal? Count_Install { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public virtual Install Install { get; set; }

        public virtual Part Part { get; set; }
    }
}
