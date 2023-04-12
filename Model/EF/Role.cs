namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        [StringLength(50)]
        public string ID { get; set; }

        [Column("Role")]
        [StringLength(255)]
        public string Role1 { get; set; }

        public bool Status { get; set; }
    }
}
