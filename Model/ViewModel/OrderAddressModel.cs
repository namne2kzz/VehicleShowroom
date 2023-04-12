using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    class OrderAddressModel
    {
        public long ID { get; set; }

        public long DealerID { get; set; }

        public long CustomerID { get; set; }
      
        public DateTime OrderedDate { get; set; }
      
        public string ReceivedName { get; set; }
      
        public string ReceivedAddress { get; set; }
        
        public string ReceivedMobile { get; set; }
     
        public string ReceivedEmail { get; set; }

        public int? Quantity { get; set; }

        public decimal? TotalCost { get; set; }

        public int Status { get; set; }
    }
}
