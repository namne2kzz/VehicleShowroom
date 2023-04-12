namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public long ID { get; set; }

        public long OrderID { get; set; }

        public long VehicleID { get; set; }

        public decimal RegistrationFee { get; set; }

        public decimal RoadMaintenanceFee { get; set; }

        public decimal CivilLiabilityInsuranceFee { get; set; }

        public decimal TestingFee { get; set; }

        public decimal RegistrationPlateFee { get; set; }

        public decimal? Total { get; set; }
    }
}
