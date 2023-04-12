using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VehicleShowroomManagementSystem.Models
{
    public class RegisterModel
    {         

        [Required(ErrorMessage = "UserName cannot be blank")]
        [StringLength(50)]      
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password cannot be blank")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Pasword length must be at least 6 characters and at most 50 characters")]      
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password cannot be blank")]    
        [Compare("Password", ErrorMessage = "Wrong confirm password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Name cannot be blank")]
        [StringLength(255)]  
        public string Name { get; set; }

       
        [Required(ErrorMessage = "BirthDay cannot be blank")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
       

        [Required(ErrorMessage = "Email cannot be blank")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone cannot be blank")]
        [RegularExpression(@"(84|91|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Phone is not valid. Phone Number must require  Viet Nam Code | India Code ")]
        [StringLength(50)]      
        public string Phone { get; set; }

       
    }
}