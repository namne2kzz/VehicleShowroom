namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Image")]
    public partial class Image
    {
        public long ID { get; set; }

        public long VehicleID { get; set; }

        [Column("Image")]
        [StringLength(50)]
        public string Image1 { get; set; }

        public bool Status { get; set; }
    }
}
