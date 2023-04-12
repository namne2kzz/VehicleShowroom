using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VehicleShowroomManagementSystem.Models
{
    public class ChangePasswordModel
    {       

        [Required(ErrorMessage = "Old Password cannot be blank")]
        [Display(Name ="Old Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password cannot be blank")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password cannot be blank")]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword",ErrorMessage ="Password not match")]
        public string ConfirmPassword { get; set; }
    }
}