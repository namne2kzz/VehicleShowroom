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
    public class BrandDAO
    {
        private VSMSDbContext db;

        public BrandDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Brand entity)
        {
            db.Brands.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }      

        public bool Update(Brand entity)
        {
            try
            {
                var brand = db.Brands.Find(entity.ID);
                brand.Name = entity.Name;
                brand.Image = entity.Image;       
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
                var brand = db.Brands.Find(id);
                db.Brands.Remove(brand);
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
                var brand = db.Brands.Find(id);
                brand.Status = !brand.Status;
                db.SaveChanges();
                return brand.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Brand> ListAll(string searchKeyword, int page, int pageSize)
        {
            IQueryable<Brand> model = db.Brands;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.Name.Contains(searchKeyword));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public List<Brand> ListAll()
        {
            return db.Brands.Where(x => x.Status == true).OrderByDescending(x => x.ID).ToList();
        }

        public Brand Detail(long id)
        {
            return db.Brands.Find(id);
        }

        public long GetIDRecent()
        {
            if (db.Brands.ToList().Count == 0)
            {
                return 0;
            }
            return db.Brands.OrderByDescending(x => x.ID).Take(1).SingleOrDefault().ID;
        }

       

        public List<BrandVehicleReport> ReportBrandVehicleSell(int month)
        {
            var brand = new BrandDAO().ListAll();
            var result = new List<BrandVehicleReport>();
            foreach (var item in brand)
            {
                var sub = new BrandVehicleReport();
                var quantity = new VehicleDAO().CountVehicleSellByBrand(null,item.ID,month);
                sub.BrandName = item.Name;
                sub.VehicleQuantity = quantity;
                result.Add(sub);
            }
            return result;
        }

        public List<BrandVehicleReport> ReportBrandVehicleSellByDealer(int month,long dealerId)
        {
            var brand = new BrandDAO().ListAll();
            var result = new List<BrandVehicleReport>();
            foreach (var item in brand)
            {
                var sub = new BrandVehicleReport();
                var quantity = new VehicleDAO().CountVehicleSellByBrand(dealerId,item.ID, month);
                sub.BrandName = item.Name;
                sub.VehicleQuantity = quantity;
                result.Add(sub);
            }
            return result;
        }

        public List<BrandVehicleReport> ReportBrandVehicleAvailable()
        {
            var brand = new BrandDAO().ListAll();
            var result = new List<BrandVehicleReport>();
            foreach (var item in brand)
            {
                var sub = new BrandVehicleReport();
                var quantity = new VehicleDAO().CountVehicleByBrand(null,item.ID);
                sub.BrandName = item.Name;
                sub.VehicleQuantity = quantity;
                result.Add(sub);
            }
            return result;
        }

        public List<BrandVehicleReport> ReportBrandVehicleAvailableByDealer(long dealerId)
        {
            var brand = new BrandDAO().ListAll();
            var result = new List<BrandVehicleReport>();
            foreach (var item in brand)
            {
                var sub = new BrandVehicleReport();
                var quantity = new VehicleDAO().CountVehicleByBrand(dealerId,item.ID);
                sub.BrandName = item.Name;
                sub.VehicleQuantity = quantity;
                result.Add(sub);
            }
            return result;
        }


    }
}
