namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Brand")]
    public partial class Brand
    {
        public long ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage ="Brand Name cannot be blank")]
        public string Name { get; set; }

        [StringLength(50)]
        public string Image { get; set; }

        public bool Status { get; set; }
    }
}
