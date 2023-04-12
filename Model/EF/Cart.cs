namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cart")]
    public partial class Cart
    {
        public long ID { get; set; }

        public long CustomerID { get; set; }

        public long VehicleID { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }
    }
}
