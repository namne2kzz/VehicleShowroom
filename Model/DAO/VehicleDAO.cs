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
    public class VehicleDAO
    {
        private VSMSDbContext db;

        public VehicleDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Vehicle entity)
        {
            db.Vehicles.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Vehicle entity)
        {
            try
            {
                var vehicle = db.Vehicles.Find(entity.ID);
                vehicle.DealerID = entity.DealerID;
                vehicle.PurchaseID = entity.PurchaseID;
                vehicle.BrandID = entity.BrandID;
                vehicle.ModelNumber = entity.ModelNumber;
                vehicle.Name = entity.Name;              
                vehicle.Description = entity.Description;
                vehicle.Detail = entity.Detail;
                vehicle.Seat = entity.Seat;
                vehicle.Style = entity.Style;
                vehicle.Color = entity.Color;
                vehicle.Model = entity.Model;
                vehicle.Origin = entity.Origin;
                vehicle.FuelType = entity.FuelType;
                vehicle.Mileage = entity.Mileage;
                vehicle.EngineDispl = entity.EngineDispl;
                vehicle.Transmission = entity.Transmission;
                vehicle.FogLamps = entity.FogLamps;
                vehicle.PowerWindow = entity.PowerWindow;
                vehicle.Airbags = entity.Airbags;
                vehicle.ABS = entity.ABS;
                vehicle.CentralLocking = entity.CentralLocking;
                vehicle.HistoricalCost = entity.HistoricalCost;
                vehicle.Price = entity.Price;
                vehicle.OwnerID = entity.OwnerID;
                vehicle.Status = entity.Status;
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
                var vehicle = db.Vehicles.Find(id);
                db.Vehicles.Remove(vehicle);
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
                var vehicle = db.Vehicles.Find(id);
                vehicle.Status = !vehicle.Status;
                db.SaveChanges();
                return vehicle.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Vehicle> ListAll(long? brandId, long dealerId, string searchKeyword, int page, int pageSize)
        {
            IQueryable<Vehicle> model = db.Vehicles.Where(x=>x.DealerID==dealerId);
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.Name.Contains(searchKeyword) || x.ModelNumber.Contains(searchKeyword) || x.Seat.ToString().Contains(searchKeyword) || x.Color.Contains(searchKeyword) || x.Style.Contains(searchKeyword) || x.Model.Contains(searchKeyword) || x.Origin.Contains(searchKeyword) || x.FuelType.Contains(searchKeyword) || x.Description.Contains(searchKeyword) || x.Detail.Contains(searchKeyword));
            }
            if (brandId != null)
            {
                return model.Where(x => x.BrandID == brandId).OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);

        }

        public List<Vehicle> ListAll(long dealerId)
        {
            return db.Vehicles.Where(x => x.DealerID == dealerId).OrderByDescending(x => x.ID).ToList();
        }      

        public List<Vehicle> ListByBrand(long brandId)
        {
            return db.Vehicles.Where(x => x.BrandID == brandId).OrderBy(x => x.ID).ToList();
        }

        public List<ImageVehicle> ListVehicleByDealer(long dealerId, ref int totalRecord, int pageIndex = 1, int pageSize = 5)
        {
            totalRecord = db.Vehicles.Where(x => x.DealerID == dealerId && x.OwnerID==null).Count();
            var model = (from a in db.Vehicles.Where(x=>x.OwnerID==null)
                         join b in db.Images
                         on a.ID equals b.VehicleID
                         where b.Status==true
                         join c in db.Discounts.Where(x=>x.StartedDate <= DateTime.Now && x.ExpiredDate >= DateTime.Now)
                         on a.ID equals c.VehicleID
                         into temp       
                         from last in temp.DefaultIfEmpty()
                         select new
                         {
                             DealerID=a.DealerID,
                             VehicleID = a.ID,
                             VehicleName=a.Name,   
                             Description=a.Description,
                             Detail=a.Detail,
                             Model = a.Model,
                             Style = a.Style,
                             Transmission = a.Transmission,
                             Price = a.Price,
                             Image = b.Image1,
                             SaleOff= last !=null? last.SaleOff : 0,
                             Status = a.Status,

                         }).AsEnumerable().Select(x => new ImageVehicle()
                         {
                             DealerID=x.DealerID,
                             VehicleID = x.VehicleID,
                             VehicleName = x.VehicleName,
                             Description=x.Description,
                             Detail=x.Detail,
                             Model = x.Model,
                             Style = x.Model,
                             Transmission = x.Transmission,
                             Price = x.Price,
                             SaleOff=x.SaleOff,
                             Image = x.Image,                          
                             Status = x.Status,
                         });
            return model.Where(x=>x.DealerID==dealerId).OrderByDescending(x => x.VehicleID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        }

        public List<ImageVehicle> ListVehicleBySearch(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 5)
        {
            totalRecord = db.Vehicles.Where(x =>x.OwnerID==null && x.Name.Contains(keyword)).Count();
            var model = db.Vehicles.Where(x => x.Name.Contains(keyword)).ToList();
            var result = (from a in model
                         join b in db.Images
                         on a.ID equals b.VehicleID
                         where b.Status == true
                         join c in db.Discounts.Where(x => x.StartedDate <= DateTime.Now && x.ExpiredDate >= DateTime.Now)
                         on a.ID equals c.VehicleID
                         into temp
                         from last in temp.DefaultIfEmpty()
                         select new
                         {
                             DealerID = a.DealerID,
                             VehicleID = a.ID,
                             VehicleName = a.Name,
                             Description = a.Description,
                             Detail = a.Detail,
                             Model = a.Model,
                             Style = a.Style,
                             Transmission = a.Transmission,
                             Price = a.Price,
                             Image = b.Image1,
                             SaleOff = last != null ? last.SaleOff : 0,
                             Status = a.Status,

                         }).AsEnumerable().Select(x => new ImageVehicle()
                         {
                             DealerID = x.DealerID,
                             VehicleID = x.VehicleID,
                             VehicleName = x.VehicleName,
                             Description = x.Description,
                             Detail = x.Detail,
                             Model = x.Model,
                             Style = x.Model,
                             Transmission = x.Transmission,
                             Price = x.Price,
                             SaleOff = x.SaleOff,
                             Image = x.Image,
                             Status = x.Status,
                         });
            return result.OrderByDescending(x => x.VehicleID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        }

        public List<Vehicle> ListToTestDrive()
        {
            return db.Vehicles.Where(x => x.Status == true && x.OwnerID==null).OrderByDescending(x => x.ID).ToList();
        }

        public Vehicle Detail(long id)
        {
            return db.Vehicles.Find(id);
        }

        public CompareViewModel GetCompareVehicle(long id)
        {
            var model = from a in db.Vehicles
                        join b in db.Brands
                        on a.BrandID equals b.ID
                        join c in db.Images
                        on a.ID equals c.VehicleID
                        where c.Status == true
                        select new CompareViewModel()
                        {
                            ID = a.ID,
                            ModelNumber = a.ModelNumber,
                            BrandID = b.ID,
                            BrandName = b.Name,
                            Name = a.Name,
                            Description = a.Description,
                            Detail = a.Detail,
                            Style = a.Style,
                            Model = a.Model,
                            Color = a.Color,
                            Seat = a.Seat,
                            Origin = a.Origin,
                            FuelType = a.FuelType,
                            Mileage = a.Mileage,
                            EngineDispl = a.EngineDispl,
                            Transmission = a.Transmission,
                            FogLamps = a.FogLamps,
                            PowerWindow = a.PowerWindow,
                            Airbags = a.Airbags,
                            ABS = a.ABS,
                            CentralLocking = a.CentralLocking,
                            Price = a.Price,
                            Image = c.Image1

                        };
            return model.Where(x=>x.ID==id).SingleOrDefault();
        }

        public VehicleDetailModel GetAllDetail(long vehicleId)
        {
            var model = (from a in db.Vehicles
                        join b in db.Brands
                        on a.BrandID equals b.ID
                        join c in db.Images
                        on a.ID equals c.VehicleID
                        where c.Status == true
                        join d in db.Dealers
                        on a.DealerID equals d.ID
                        join e in db.Discounts.Where(x => x.StartedDate <= DateTime.Now && x.ExpiredDate >= DateTime.Now)
                        on a.ID equals e.VehicleID
                        into temp
                        from last in temp.DefaultIfEmpty()
                        select new VehicleDetailModel()
                        {
                            ID = a.ID,
                            ModelNumber = a.ModelNumber,
                            DealerID=a.DealerID,
                            DealerName=d.DealerName,
                            BrandID = b.ID,
                            BrandName = b.Name,
                            Name = a.Name,
                            Description = a.Description,
                            Detail = a.Detail,
                            Style = a.Style,
                            Model = a.Model,
                            Color = a.Color,
                            Seat = a.Seat,
                            Origin = a.Origin,
                            FuelType = a.FuelType,
                            Mileage = a.Mileage,
                            EngineDispl = a.EngineDispl,
                            Transmission = a.Transmission,
                            FogLamps = a.FogLamps,
                            PowerWindow = a.PowerWindow,
                            Airbags = a.Airbags,
                            ABS = a.ABS,
                            CentralLocking = a.CentralLocking,
                            Price = a.Price,
                            Image = c.Image1,
                            SaleOff= last != null ? last.SaleOff : 0,
                            OwnerID=a.OwnerID,
                            Status=a.Status
                        }).AsEnumerable().Select(x=>new VehicleDetailModel() {
                            ID = x.ID,
                            ModelNumber = x.ModelNumber,
                            DealerID = x.DealerID,
                            DealerName = x.DealerName,
                            BrandID = x.ID,
                            BrandName = x.BrandName,
                            Name = x.Name,
                            Description = x.Description,
                            Detail = x.Detail,
                            Style = x.Style,
                            Model = x.Model,
                            Color = x.Color,
                            Seat = x.Seat,
                            Origin = x.Origin,
                            FuelType = x.FuelType,
                            Mileage = x.Mileage,
                            EngineDispl = x.EngineDispl,
                            Transmission = x.Transmission,
                            FogLamps = x.FogLamps,
                            PowerWindow = x.PowerWindow,
                            Airbags = x.Airbags,
                            ABS = x.ABS,
                            CentralLocking = x.CentralLocking,
                            Price = x.Price,
                            Image = x.Image,
                            SaleOff = x.SaleOff,
                            OwnerID = x.OwnerID,
                            Status = x.Status
                        });
            return model.Where(x => x.ID == vehicleId && x.OwnerID==null  && x.Status==true).SingleOrDefault();
        }

        public List<ImageVehicle> ListRelated(long vehicleId)
        {
            var vehicle = new VehicleDAO().Detail(vehicleId);
            var model = from a in db.Vehicles.Where(x=>x.OwnerID==null)                      
                        join c in db.Images
                        on a.ID equals c.VehicleID
                        where c.Status == true
                        select new ImageVehicle()
                        {
                            DealerID=a.DealerID,
                            VehicleID = a.ID,
                            ModelNumber = a.ModelNumber,                         
                            VehicleName = a.Name,                          
                            Style = a.Style,
                            Model = a.Model,
                            Color = a.Color,                          
                            Price = a.Price,
                            Image = c.Image1

                        };
            return model.Where(x => x.VehicleID !=vehicleId && x.DealerID == vehicle.DealerID).ToList();
        }

        public bool CheckModelNumberExist(string modelNumber)
        {
            return db.Vehicles.Count(x => x.ModelNumber == modelNumber) > 0;
        }

        public List<Vehicle> ListBill(long purchaseId)
        {
            return db.Vehicles.Where(x => x.PurchaseID == purchaseId).OrderBy(x => x.ID).ToList();
        }

        public List<Vehicle> ListVehicleRegistration(long dealerId)
        {
            return db.Vehicles.Where(x => x.DealerID==dealerId && x.OwnerID != null && x.Status == true).ToList();
        }

        public int CountVehicleSellByBrand(long? dealerId,long brandId,int month)
        {
            
            var model = from a in db.Vehicles
                        join b in db.Brands
                        on a.BrandID equals b.ID
                        join c in db.OrderDetails
                        on a.ID equals c.VehicleID
                        join d in db.Orders
                        on c.OrderID equals d.ID
                        select new
                        {
                            DealerID=a.DealerID,
                            BrandID=b.ID,
                            BrandName=b.Name,
                            Vehicle=a.Name,
                            OrderedDate=d.OrderedDate,
                        };
            if (dealerId != null)
            {
                return model.Where(x => x.BrandID == brandId && x.DealerID==dealerId && x.OrderedDate.Month == month).Count();
            }
            return model.Where(x => x.BrandID == brandId && x.OrderedDate.Month==month).Count();
            
        }

        public int CountVehicleByBrand(long? dealerId, long brandId)
        {
            if(dealerId != null)
            {
                return db.Vehicles.Count(x => x.BrandID == brandId && x.DealerID==dealerId && x.OwnerID == null);
            }
            return db.Vehicles.Count(x => x.BrandID == brandId && x.OwnerID ==null);
        }

        public List<string> ListName(string keyword)
        {
            return db.Vehicles.Where(x => x.Name.Contains(keyword) && x.OwnerID==null).Select(x => x.Name).ToList();
        }

        public bool UpdateOwnerID(long vehicleid, long customerId)
        {
            try
            {
                var vehicle = new VehicleDAO().Detail(vehicleid);
                vehicle.OwnerID = customerId;
                new VehicleDAO().Update(vehicle);
                db.SaveChanges();
                
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
    }
}
