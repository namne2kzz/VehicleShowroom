namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Blog")]
    public partial class Blog
    {
        public long ID { get; set; }

        public long DealerID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Blog Name cannot be blank")]
        [Display(Name ="Blog Name")]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Description cannot be blank")]
        public string Description { get; set; }    

        [StringLength(50)]       
        public string Image { get; set; }      

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        public int Status { get; set; }
    }
}
