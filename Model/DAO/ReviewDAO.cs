using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;

namespace Model.DAO
{
    public class ReviewDAO
    {
        private VSMSDbContext db;

        public ReviewDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Review entity)
        {
            db.Reviews.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }     

        public bool Delete(long id)
        {
            try
            {
                var review = db.Reviews.Find(id);
                db.Reviews.Remove(review);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }   
     

        public List<ReviewCustomer> ListByVehicle(long vehicleId)
        {
            var model = from a in db.Reviews.Where(x=>x.VehicleID==vehicleId)
                        join b in db.Customers
                        on a.CustomerID equals b.ID
                        select new ReviewCustomer()
                        {
                            ID = a.ID,
                            VehicleID = a.VehicleID,
                            CustomerID = a.CustomerID,
                            CustomerName = b.Name,
                            Avater = b.Avatar,
                            Content = a.Content,
                            CreatedDate = a.CreatedDate,
                            Status = a.Status,
                        };
            return model.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public Review Detail(long id)
        {
            return db.Reviews.Find(id);
        }

        public int CountByVehicle(long id)
        {
            return db.Reviews.Count(x => x.VehicleID == id);
        }
    }
}
