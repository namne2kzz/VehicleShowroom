using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class OrderDetailDAO
    {
        private VSMSDbContext db;
        public OrderDetailDAO()
        {
            db = new VSMSDbContext();
        }

        public List<OrderDetail> ListAll(long orderId)
        {
            return db.OrderDetails.Where(x => x.OrderID == orderId).OrderBy(x=>x.ID).ToList();
        }
        
        public OrderDetail Detail(long id)
        {
            return db.OrderDetails.Find(id);
        }

        public long Insert(OrderDetail entity)
        {
            try
            {
                db.OrderDetails.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch(Exception ex)
            {
                return -3;
            }
         
        }
            
    }
}
