using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;
using PagedList;

namespace Model.DAO
{
    public class FeatureDAO
    {

        private VSMSDbContext db;

        public FeatureDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Feature entity)
        {
            db.Features.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Feature entity)
        {
            try
            {
                var feature = db.Features.Find(entity.ID);
                feature.VehicleID = entity.VehicleID;                
                feature.StartedDate = entity.StartedDate;
                feature.ExpiredDate = entity.ExpiredDate;              
                feature.Status = entity.Status;
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
                var feature = db.Features.Find(id);
                db.Features.Remove(feature);
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
                var feature = db.Features.Find(id);
                feature.Status = !feature.Status;
                db.SaveChanges();
                return feature.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<FeatureVehicle> ListAll(long dealerId, string searchKeyword, int page, int pageSize)
        {
            IQueryable<FeatureVehicle> model = from a in db.Features
                                               join b in db.Vehicles.Where(x => x.DealerID == dealerId)
                                               on a.VehicleID equals b.ID
                                               select new FeatureVehicle()
                                               {
                                                   ID=a.ID,
                                                   VehicleID=a.VehicleID,
                                                   VehicleName=b.Name,
                                                   StartedDate=a.StartedDate,
                                                   ExpiredDate=a.ExpiredDate,
                                                   Status=a.Status
                                               };
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.VehicleName.Contains(searchKeyword) || x.StartedDate.ToString().Contains(searchKeyword) || x.ExpiredDate.ToString().Contains(searchKeyword));
            }
            return model.OrderByDescending(x => x.StartedDate).ToPagedList(page, pageSize);
        }

        public Feature Detail(long id)
        {
            return db.Features.Find(id);
        }

        public List<FeatureVehicle> ListFeature()
        {
            var model = from a in db.Vehicles.Where(x=>x.OwnerID==null)
                        join b in db.Features.Where(x => x.StartedDate <= DateTime.Now && x.ExpiredDate >= DateTime.Now)
                        on a.ID equals b.VehicleID
                        join c in db.Images
                        on b.ID equals c.VehicleID
                        where c.Status == true
                        select new FeatureVehicle()
                        {                         
                            VehicleID = b.VehicleID,
                            VehicleName = a.Name,
                            StartedDate = b.StartedDate,
                            ExpiredDate = b.ExpiredDate,
                            Style = a.Style,
                            Model = a.Model,
                            Color = a.Color,
                            Price = a.Price,
                            Image = c.Image1,
                            Status=b.Status,

                        };
            return model.OrderByDescending(x => x.StartedDate).ToList();
        }

        public bool CheckFeatureVehicle(long vehicleId)
        {
            return db.Features.Where(x => x.VehicleID == vehicleId && x.StartedDate <= DateTime.Now && x.ExpiredDate >= DateTime.Now).ToList().Count > 0;
        }

       
    }
}
