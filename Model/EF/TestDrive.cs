namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestDrive")]
    public partial class TestDrive
    {
        public long ID { get; set; }

        public long VehicleID { get; set; }

        public long CustomerID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }

        [Required(ErrorMessage = "BirthDay cannot be blank")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Email must not empty")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Phone must not empty")]
        [RegularExpression(@"(84|91|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Phone is not valid. Phone Number must require Viet Nam Code | India Code")]
        public string Phone { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Address cannot be blank")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Registration Date cannot be blank")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Registration")]
        [Column(TypeName = "date")]
        public DateTime RegistrationDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Driver's License")]
        [Required(ErrorMessage = "Driver's License cannot be blank")]
        public string DriverLicense { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "More Request")]
        [Required(ErrorMessage = "More Request cannot be blank")]
        public string MoreRequest { get; set; }

        public int Status { get; set; }
    }
}
