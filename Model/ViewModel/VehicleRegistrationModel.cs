using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class VehicleRegistrationModel
    {
        public long ID { get; set; }

        public long DealerID { get; set; }

        public long VehicleID { get; set; }

        public string VehicleName { get; set; }

        public string ModelNumber { get; set; }

        public long CustomerID { get; set; }
       
        public string OwnerName { get; set; }
       
        public DateTime DateOfBirth { get; set; }
      
        public string IdentityCardNumber { get; set; }
       
        public string PlateNumber { get; set; }

        public string RegistrationPlace { get; set; }
      
        public string Email { get; set; }
        
        public string Phone { get; set; }
       
        public string Address { get; set; }

        public bool Status { get; set; }
    }
}
