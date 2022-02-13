using Designer_Offer.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Builds : IEntity
    {
        public int Id { get; set; }

        public int? Client_Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Adress { get; set; }

        public virtual Clients Clients { get; set; }

        public virtual Projects Projects { get; set; }
    }
}
