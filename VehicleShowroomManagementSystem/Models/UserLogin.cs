using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleShowroomManagementSystem.Models
{
    [Serializable]
    public class UserLogin
    {
        public long UserID { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public string RoleID { get; set; }
    }
}