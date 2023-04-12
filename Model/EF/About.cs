namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("About")]
    public partial class About
    {
        public long ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Email cannot be blank")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Phone mustn't empty")]
        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Phone is not valid. Phone Number must require Viet Nam Code ")]
        public string Phone { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Address cannot be blank")]
        public string Address { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Description cannot be blank")]
        public string Description { get; set; }    

        public bool Status { get; set; }
    }
}
