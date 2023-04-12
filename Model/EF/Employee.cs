namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        public long ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Name cannot be blank")]
        [Display(Name ="Employee Name")]
        public string Name { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "UserName cannot be blank")]
        public string UserName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Password cannot be blank")]
        public string Password { get; set; }

        [StringLength(50)]
        public string Avatar { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "Date Of Birth cannot be blank")]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

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

        [StringLength(50)]
        public string RoleID { get; set; }

        public bool Status { get; set; }
    }
}
