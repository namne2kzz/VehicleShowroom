using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.ViewModel
{
    public class OrderDetailModel
    {
        public long OrderID { get; set; }

        public long DealerID { get; set; }

        public long VehicleID { get; set; }

        public string ModelNumber { get; set; }

        public string VehicleName { get; set; }
       
        public decimal Price { get; set; }

        public string Image { get; set; }

        public double SaleOff { get; set; }

        public decimal RegistrationFee { get; set; }

        public decimal RoadMaintennanceFee { get; set; }

        public decimal CivilLiabilityInsuranceFee { get; set; }

        public decimal TestingFee { get; set; }

        public decimal RegistrationPlateFee { get; set; }

        public bool Status { get; set; }
    }
}