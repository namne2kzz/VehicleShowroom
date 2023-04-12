namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public long ID { get; set; }

        public long DealerID { get; set; }

        public long CustomerID { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime OrderedDate { get; set; }
       
        [StringLength(255)]
        [Required(ErrorMessage = "Received Name cannot be blank")]
        public string ReceivedName { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Received Address cannot be blank")]
        public string ReceivedAddress { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Received Mobile cannot be blank")]
        public string ReceivedMobile { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Received Email cannot be blank")]
        public string ReceivedEmail { get; set; }

        public int Quantity { get; set; }

        public decimal TotalCost { get; set; }

        public int Status { get; set; }
    }
}
