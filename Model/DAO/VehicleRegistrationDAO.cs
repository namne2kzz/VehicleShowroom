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
    public class VehicleRegistrationDAO
    {
        private VSMSDbContext db;

        public VehicleRegistrationDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(VehicleRegistrationData entity)
        {
            db.VehicleRegistrationDatas.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(VehicleRegistrationData entity)
        {
            try
            {
                var vehicleRegistrationData = db.VehicleRegistrationDatas.Find(entity.ID);
                vehicleRegistrationData.CustomerID = entity.CustomerID;
                vehicleRegistrationData.VehicleID = entity.VehicleID;
                vehicleRegistrationData.OwnerName = entity.OwnerName;
                vehicleRegistrationData.DateOfBirth = entity.DateOfBirth;
                vehicleRegistrationData.Email = entity.Email;
                vehicleRegistrationData.Phone = entity.Phone;
                vehicleRegistrationData.RegistrationPlace = entity.RegistrationPlace;
                vehicleRegistrationData.RegistrationPlace = entity.RegistrationPlace;
                vehicleRegistrationData.PlateNumber = entity.PlateNumber;
                vehicleRegistrationData.Status = entity.Status;
                vehicleRegistrationData.IdentityCardNumber = entity.IdentityCardNumber;
                vehicleRegistrationData.Address = entity.Address;                   
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
                var vehicleRegistration = db.VehicleRegistrationDatas.Find(id);
                db.VehicleRegistrationDatas.Remove(vehicleRegistration);
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
                var vehicleRegistration = db.VehicleRegistrationDatas.Find(id);
                vehicleRegistration.Status = !vehicleRegistration.Status;
                db.SaveChanges();
                return vehicleRegistration.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<VehicleRegistrationModel> ListAll(long dealerId, string searchKeyword, int page, int pageSize)
        {
            IQueryable<VehicleRegistrationModel> model = from a in db.Vehicles
                                                        join b in db.Dealers
                                                        on a.DealerID equals b.ID
                                                        join c in db.VehicleRegistrationDatas
                                                        on a.ID equals c.VehicleID
                                                        select new VehicleRegistrationModel()
                                                        {
                                                            ID=c.ID,
                                                            DealerID=b.ID,
                                                            VehicleID=a.ID,
                                                            VehicleName=a.Name,
                                                            ModelNumber=a.ModelNumber,
                                                            CustomerID=c.CustomerID,
                                                            OwnerName=c.OwnerName,
                                                            DateOfBirth=c.DateOfBirth,
                                                            IdentityCardNumber=c.IdentityCardNumber,
                                                            PlateNumber=c.PlateNumber,
                                                            RegistrationPlace=c.RegistrationPlace,
                                                            Email=c.Email,
                                                            Phone=c.Phone,
                                                            Address=c.Address,
                                                            Status=c.Status,
                                                        };
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.DealerID==dealerId && x.OwnerName.Contains(searchKeyword) || x.PlateNumber.Contains(searchKeyword) || x.RegistrationPlace.Contains(searchKeyword));
            }
            return model.Where(x=>x.DealerID==dealerId).OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }    

        public VehicleRegistrationData Detail(long id)
        {
            return db.VehicleRegistrationDatas.Find(id);
        }

        public IEnumerable<AllotmentVehicleRegistrationReport> ListCountVehicleRegistrationByProvince()
        {
            IQueryable<AllotmentVehicleRegistrationReport> result = db.VehicleRegistrationDatas.GroupBy(x => x.RegistrationPlace).Select(x => new AllotmentVehicleRegistrationReport()
            {
                Province = x.Key,
                Quantity = x.Count()
            });
            return result;
        }
    }
}
