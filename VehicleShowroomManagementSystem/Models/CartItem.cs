using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleShowroomManagementSystem.Models
{
    [Serializable]
    public class CartItem
    {
        public Vehicle Vehicle { get; set; }

        public string Image { get; set; }

        public double SaleOff { get; set; }
    }
}