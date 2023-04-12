using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class CartDAO
    {

        private VSMSDbContext db;

        public CartDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Cart entity)
        {
            try
            {
                db.Carts.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch(Exception ex)
            {
                return -1;
            }
           
        }
       

        public bool Delete(long id)
        {
            try
            {
                var cart = db.Carts.Find(id);
                db.Carts.Remove(cart);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Cart Detail(long vehicleId)
        {
            return db.Carts.Where(x => x.VehicleID == vehicleId).FirstOrDefault();
        }

        public Cart CartByCustomer(long customerId)
        {
            return db.Carts.Where(x => x.CustomerID == customerId).First();
        }

        public List<Cart> ListCartByCustomer(long customerId)
        {
            return db.Carts.Where(x => x.CustomerID == customerId).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<Cart> ListCartByCustomerByDealer(long customerId, long dealerId)
        {
            var cart= db.Carts.Where(x => x.CustomerID == customerId).OrderByDescending(x => x.CreatedDate).ToList();
            var vehicle = db.Vehicles.Where(x => x.DealerID == dealerId).ToList();
            var listCart = new List<Cart>();
            foreach (var subVehicle in vehicle)
            {
                foreach (var subCart in cart)
                {
                    if (subVehicle.ID == subCart.VehicleID)
                    {
                        listCart.Add(subCart);
                    }
                }
            }
            return listCart;
        }
    

        public int CountCartByCustomer(long customerId)
        {
            return db.Carts.Count(x => x.CustomerID == customerId);
        }

        public bool CheckVehicleExistCart(long customerId, long vehicleId)
        {
            if(db.Carts.Where(x=>x.CustomerID==customerId && x.VehicleID == vehicleId).Count() > 0)
            {
                return true;
            }
            return false;
        }

        public List<Cart> ListAll()
        {
            return db.Carts.ToList();
        }        
    }
}
