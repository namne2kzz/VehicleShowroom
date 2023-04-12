namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Discount")]
    public partial class Discount
    {
        public long ID { get; set; }

        public long VehicleID { get; set; }

        [Column(TypeName = "date")]
        [Required(ErrorMessage = "Started Date cannot be blank")]
        [Display(Name ="Started Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartedDate { get; set; }

        [Column(TypeName = "date")]
        [Required(ErrorMessage = "Expired Date cannot be blank")]
        [Display(Name = "Expired Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ExpiredDate { get; set; }

        [Required(ErrorMessage = "Sale Off cannot be blank")]
        [Display(Name = "Sale Off")]
        [Range(0,1,ErrorMessage ="Discount Value must be in range 0 to 1")]
        public double SaleOff { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Description cannot be blank")]
        public string Description { get; set; }

        public bool Status { get; set; }
    }
}
