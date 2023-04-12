using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class DiscountVehicle
    {
        public long ID { get; set; }
       
        public long VehicleID { get; set; }

        public string VehicleName { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpiredDate { get; set; }
      
        public double SaleOff { get; set; }
      
        public string Description { get; set; }

        public string Style { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public bool Status { get; set; }
    }
}
