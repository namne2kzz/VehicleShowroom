namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MandatoryCost")]
    public partial class MandatoryCost
    {
        public long ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Description cannot be blank")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cost cannot be blank")]
        [Range(0,int.MaxValue,ErrorMessage ="Cost Value is not valid")]
        public decimal Cost { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Unit cannot be blank")]
        public string Unit { get; set; }

        public bool Status { get; set; }
    }
}
