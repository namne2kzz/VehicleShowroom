namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ServiceOrderDetail")]
    public partial class ServiceOrderDetail
    {
        public long ID { get; set; }

        public long OrderID { get; set; }

        public long ServiceID { get; set; }
    }
}
