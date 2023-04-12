using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class RoleDAO
    {
        private VSMSDbContext db;

        public RoleDAO()
        {
            db = new VSMSDbContext();
        }

        public List<Role> ListAll()
        {
            return db.Roles.Where(x => x.Status == true).ToList();
        }

        public Role GetById(string id)
        {
            return db.Roles.Find(id);
        }

    }
}
