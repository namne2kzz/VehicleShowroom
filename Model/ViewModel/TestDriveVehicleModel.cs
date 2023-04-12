using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class TestDriveVehicleModel
    {
        public long ID { get; set; }

        public long VehicleID { get; set; }

        public string VehicleName { get; set; }

        public string ModelNumber { get; set; }

        public long CustomerID { get; set; }
       
        public string Name { get; set; }
       
        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    
        public string Address { get; set; }

        public DateTime RegistrationDate { get; set; }
       
        public string DriverLicense { get; set; }
       
        public string MoreRequest { get; set; }

        public int Status { get; set; }
    }
}
