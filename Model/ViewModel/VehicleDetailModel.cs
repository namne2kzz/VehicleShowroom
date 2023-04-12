using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class VehicleDetailModel
    {
        public long ID { get; set; }

        public long DealerID { get; set; }

        public string DealerName { get; set; }

        public long BrandID { get; set; }

        public string BrandName { get; set; }

        public string ModelNumber { get; set; }

        public string Name { get; set; }

        public string MetaTitle { get; set; }

        public string Description { get; set; }

        public string Detail { get; set; }

        public int Seat { get; set; }

        public string Style { get; set; }

        public string Color { get; set; }

        public string Model { get; set; }

        public string Origin { get; set; }

        public string FuelType { get; set; }

        public string Mileage { get; set; }

        public string EngineDispl { get; set; }

        public string Transmission { get; set; }

        public string FogLamps { get; set; }

        public bool? PowerWindow { get; set; }

        public bool? Airbags { get; set; }

        public bool? ABS { get; set; }

        public bool? CentralLocking { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public double SaleOff { get; set; }

        public long? OwnerID { get; set; }

        public bool Status { get; set; }
    }
}
