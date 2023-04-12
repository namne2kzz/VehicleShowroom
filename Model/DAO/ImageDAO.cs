using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.DAO
{
    public class ImageDAO
    {
        private VSMSDbContext db;
        public ImageDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Image entity)
        {
            db.Images.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Image entity)
        {
            try
            {
                var image = db.Images.Find(entity.ID);
                image.Image1 = entity.Image1;
                image.Status = entity.Status;
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
                var image = db.Images.Find(id);
                db.Images.Remove(image);
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
                var image = db.Images.Find(id);
                image.Status = !image.Status;
                db.SaveChanges();
                return image.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Image> ListAll(long vehicleId)
        {
            return db.Images.Where(x => x.VehicleID == vehicleId).ToList();
        }

        public long CountImage(long vehicleId)
        {
            if (db.Images.Where(x=>x.VehicleID==vehicleId).ToList().Count == 0)
            {
                return 0;
            }
            return db.Images.Where(x => x.VehicleID==vehicleId).ToList().Count;
        }

        public Image Detail(long id)
        {
            return db.Images.Find(id);
        }

        public Image GetMainImage(long vehicleId)
        {
            return db.Images.Where(x => x.VehicleID == vehicleId && x.Status == true).SingleOrDefault();
        }

        public List<Image> ListImageFalse(long vehicleId)
        {
            return db.Images.Where(x => x.VehicleID == vehicleId && x.Status == false).ToList();
        }
    }
}
