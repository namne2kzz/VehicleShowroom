using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
     public class AddressDAO
    {
        private VSMSDbContext db;

        public AddressDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Address entity)
        {
            db.Addresses.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }        

        public bool Delete(long id)
        {
            try
            {
                var address = db.Addresses.Find(id);
                db.Addresses.Remove(address);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            try
            {
                var about = db.Addresses.Find(id);
                about.Status = !about.Status;
                db.SaveChanges();
                return about.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       

        public List<Address> ListAll(long custometId)
        {
            return db.Addresses.Where(x=>x.CustomerID==custometId).OrderByDescending(x => x.ID).ToList();
        }

        public Address Detail(long id)
        {
            return db.Addresses.Find(id);
        }

        public long CountAddress(long customerId)
        {
            if (db.Addresses.Where(x => x.CustomerID == customerId).ToList().Count == 0)
            {
                return 0;
            }
            return db.Addresses.Where(x => x.CustomerID == customerId).ToList().Count;
        }

        public List<Address> ListByCustomer(long customerId)
        {
            return db.Addresses.Where(x => x.CustomerID == customerId).ToList();

        }
    }
}
