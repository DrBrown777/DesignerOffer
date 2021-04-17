namespace Designer_Offer.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserData")]
    public partial class UserData
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Employee_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Login { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(32)]
        public string Password { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
