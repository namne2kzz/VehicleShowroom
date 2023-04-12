namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address")]
    public partial class Address
    {
        public long ID { get; set; }

        public long? CustomerID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage ="District cannot be blank")]
        public string District { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Province cannot be blank")]
        public string Province { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Address Detail cannot be blank")]
        [Display(Name ="Address Detail")]
        public string AddressDetail { get; set; }

        public bool Status { get; set; }
    }
}
