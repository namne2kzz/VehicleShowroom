namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Service")]
    public partial class Service
    {
        public long ID { get; set; }

        public long DealerID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Sercive Name cannot be blank")]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Description cannot be blank")]
        public string Description { get; set; }

        public bool Status { get; set; }
    }
}
