namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
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
        [Required(ErrorMessage = "Phone cannot be blank")]
        [RegularExpression(@"(84|91|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Phone is not valid. Phone Number must require Viet Nam Code | India Code")]
        public string Phone { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Content cannot be blank")]
        public string Content { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Province cannot be blank")]
        public string Province { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }
    }
}
