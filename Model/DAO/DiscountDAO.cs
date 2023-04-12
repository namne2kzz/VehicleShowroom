using Model.EF;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class DiscountDAO
    {
        private VSMSDbContext db;

        public DiscountDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Discount entity)
        {
            db.Discounts.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Discount entity)
        {
            try
            {
                var discount = db.Discounts.Find(entity.ID);
                discount.VehicleID = entity.VehicleID;
                discount.Description = entity.Description;
                discount.StartedDate = entity.StartedDate;
                discount.ExpiredDate = entity.ExpiredDate;
                discount.SaleOff = entity.SaleOff;
                discount.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                var discount = db.Discounts.Find(id);
                db.Discounts.Remove(discount);
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
                var discount = db.Discounts.Find(id);
                discount.Status = !discount.Status;
                db.SaveChanges();
                return discount.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<DiscountVehicle> ListAll(long dealerId, string searchKeyword, int page, int pageSize)
        {
            IQueryable<DiscountVehicle> model = from a in db.Discounts
                                                join b in db.Vehicles.Where(x => x.DealerID == dealerId)
                                                on a.VehicleID equals b.ID
                                                select new DiscountVehicle()
                                                {
                                                    ID = a.ID,
                                                    VehicleID = a.VehicleID,
                                                    VehicleName = b.Name,
                                                    Description = a.Description,
                                                    StartedDate = a.StartedDate,
                                                    ExpiredDate = a.ExpiredDate,
                                                    SaleOff=a.SaleOff,
                                                    Status = a.Status,
                                                };
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.Description.Contains(searchKeyword) || x.VehicleName.Contains(searchKeyword) || x.StartedDate.ToString().Contains(searchKeyword) || x.ExpiredDate.ToString().Contains(searchKeyword));
            }
            return model.OrderByDescending(x => x.StartedDate).ToPagedList(page, pageSize);
        }      

        public Discount Detail(long id)
        {
            return db.Discounts.Find(id);
        }       

        public List<DiscountVehicle> ListDiscount()
        {
            var model = from a in db.Vehicles.Where(x=>x.OwnerID==null)
                        join b in db.Discounts.Where(x => x.StartedDate <= DateTime.Now && x.ExpiredDate >= DateTime.Now)
                        on a.ID equals b.VehicleID
                        join c in db.Images
                        on a.ID equals c.VehicleID
                        where c.Status==true
                        select new DiscountVehicle()
                        {
                           
                            VehicleID = b.VehicleID,
                            VehicleName = a.Name,
                            Description = b.Description,
                            StartedDate=b.StartedDate,
                            ExpiredDate=b.ExpiredDate,
                            Model=a.Model,
                            Style=a.Style,                           
                            Color=a.Color,
                            Price=a.Price,
                            SaleOff=b.SaleOff,
                            Image=c.Image1,
                            Status=b.Status,
                        };
            return model.OrderByDescending(x => x.StartedDate).ToList();
        }

        public bool CheckDiscountVehicle(long vehicleId)
        {
            return db.Discounts.Where(x => x.VehicleID == vehicleId && x.StartedDate <= DateTime.Now && x.ExpiredDate >= DateTime.Now).ToList().Count > 0;
        }

        public double GetDiscountVehicle(long vehicleId)
        {
            var model = db.Discounts.Where(x => x.VehicleID == vehicleId && x.StartedDate <=DateTime.Now && x.ExpiredDate >=DateTime.Now ).SingleOrDefault();
           if(model != null)
            {
                return model.SaleOff;
            }
            return 0;
        }
    }
}
