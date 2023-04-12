namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Slide")]
    public partial class Slide
    {
        public long ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Title cannot be blank")]
        public string Title { get; set; }

        [StringLength(50)]      
        public string Image { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Desciption cannot be blank")]
        public string Description { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Detail cannot be blank")]
        public string Detail { get; set; }

        [StringLength(50)]     
        public string Type { get; set; }

        public bool Status { get; set; }
    }
}
