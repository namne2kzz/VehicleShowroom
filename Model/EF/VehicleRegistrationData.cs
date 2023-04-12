namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VehicleRegistrationData")]
    public partial class VehicleRegistrationData
    {
        public long ID { get; set; }

        public long VehicleID { get; set; }

        public long CustomerID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Owner Name cannot be blank")]
        public string OwnerName { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date Of Birth cannot be blank")]
        [Display(Name ="Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Identity Card Number cannot be blank")]
        [Display(Name = "Identity Card Number")]
        public string IdentityCardNumber { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Plate Number cannot be blank")]
        [Display(Name = "Plate Number")]
        public string PlateNumber { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Registration Place cannot be blank")]
        [Display(Name = "Resgistration Place")]
        public string RegistrationPlace { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Email cannot be blank")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Phone cannot be blank")]
        [RegularExpression(@"(84|91|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Phone is not valid. Phone Number must require Viet Nam Code | India Code")]
        public string Phone { get; set; }     

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Address cannot be blank")]
        public string Address { get; set; }

        public bool Status { get; set; }
    }
}
