namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Feature")]
    public partial class Feature
    {
        public long ID { get; set; }

        public long VehicleID { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Started Date cannot be blank")]
        [Display(Name = "Started Date")]
        public DateTime StartedDate { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Expired Date cannot be blank")]
        [Display(Name = "Expired Date")]
        public DateTime ExpiredDate { get; set; }

        public bool Status { get; set; }
    }
}
