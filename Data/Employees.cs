using Designer_Offer.Models.Base;
using Designer_Offer.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Designer_Offer.Data
{
    public partial class Employees : Model, IEntity
    {
        public Employees()
        {
            Projects = new HashSet<Projects>();
        }

        public int Id { get; set; }

        public int? Company_Id { get; set; }

        public int? Position_Id { get; set; }

        private string _First_Name;
        [Required]
        [StringLength(50)]
        public string First_Name 
        { 
            get => _First_Name; 
            set => Set(ref _First_Name, value); 
        }

        private string _Last_Name;
        [Required]
        [StringLength(50)]
        public string Last_Name 
        { 
            get => _Last_Name; 
            set => Set(ref _Last_Name, value); 
        }

        private string _Phone;
        [Required]
        [StringLength(20)]
        public string Phone
        {
            get => _Phone; 
            set => Set(ref _Phone, value); 
        }

        private string _Mail;
        [Required]
        [StringLength(30)]
        public string Mail 
        { 
            get => _Mail; 
            set => Set(ref _Mail, value); 
        }

        public virtual Companies Companies { get; set; }

        public virtual Positions Positions { get; set; }

        public virtual ICollection<Projects> Projects { get; set; }

        public virtual UsersData UsersData { get; set; }
    }
}
