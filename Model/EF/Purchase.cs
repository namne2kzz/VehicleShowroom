namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Purchase")]
    public partial class Purchase
    {
        public long ID { get; set; }

        public long? DealerID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Received Name cannot be blank")]
        [Display(Name ="Received Name")]
        public string ReceivedName { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Received Address cannot be blank")]
        [Display(Name = "Received Address")]
        public string ReceivedAddress { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Received Email cannot be blank")]
        [Display(Name = "Received Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string ReceivedEmail { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Received Phone cannot be blank")]
        [Display(Name = "Received Phone")]
        [RegularExpression(@"(84|91|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Phone is not valid. Phone Number must require Viet Nam Code | India Code")]
        public string ReceivedPhone { get; set; }

        public int Quantity { get; set; }

        public decimal TotalCost { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime? ReceivedDate { get; set; }

        public int Status { get; set; }
    }
}
