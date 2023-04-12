using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ReviewCustomer
    {
        public long ID { get; set; }

        public long VehicleID { get; set; }

        public long CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string Avater { get; set; }

        public string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }
    }
}
