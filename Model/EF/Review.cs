namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Review")]
    public partial class Review
    {
        public long ID { get; set; }

        public long VehicleID { get; set; }

        public long CustomerID { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Content cannot be blank")]
        public string Content { get; set; }     

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }
    }
}
