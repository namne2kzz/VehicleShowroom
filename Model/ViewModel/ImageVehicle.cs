using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ImageVehicle
    {      
        public long OrderID { get; set; }

        public long DealerID { get; set; }

        public long? VehicleID { get; set; }

        public string ModelNumber { get; set; }

        public string VehicleName { get; set; }

        public string Description { get; set; }

        public string Detail { get; set; }

        public string Style { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string Transmission { get; set; }

        public decimal Price { get; set; } 

        public string Image { get; set; }

        public double SaleOff { get; set; }       

        public bool Status { get; set; }
    }
}
