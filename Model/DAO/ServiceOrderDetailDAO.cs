using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class ServiceOrderDetailDAO
    {
        private VSMSDbContext db;

        public ServiceOrderDetailDAO()
        {
            db = new VSMSDbContext();
        }

        public List<ServiceOrderDetail> ListAll(long orderId)
        {
            return db.ServiceOrderDetails.Where(x => x.OrderID == orderId).OrderBy(x => x.ID).ToList();
        }

        public ServiceOrderDetail Detail(long id)
        {
            return db.ServiceOrderDetails.Find(id);
        }

        public long Insert(ServiceOrderDetail entity)
        {
            try
            {
                db.ServiceOrderDetails.Add(entity);
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
