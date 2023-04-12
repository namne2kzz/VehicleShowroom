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
    public class TestDriveDAO
    {
        private VSMSDbContext db;
        public TestDriveDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(TestDrive entity)
        {
            db.TestDrives.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(TestDrive entity)
        {
            try
            {
                var testDrive = db.TestDrives.Find(entity.ID);
                testDrive.CustomerID = entity.CustomerID;
                testDrive.VehicleID = entity.VehicleID;
                testDrive.Name = entity.Name;
                testDrive.DateOfBirth = entity.DateOfBirth;
                testDrive.Email = entity.Email;
                testDrive.Phone = entity.Phone;
                testDrive.Address = entity.Address;
                testDrive.RegistrationDate = entity.RegistrationDate;
                testDrive.DriverLicense = entity.DriverLicense;
                testDrive.MoreRequest = entity.MoreRequest;               
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
                var testDrive = db.TestDrives.Find(id);
                db.TestDrives.Remove(testDrive);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int ChangeStatus(long id, int status)
        {
            try
            {
                var testDrive = db.TestDrives.Find(id);
                testDrive.Status = status;
                db.SaveChanges();
                return testDrive.Status;
            }
            catch (Exception ex)
            {
                return -3;
            }
        }

        public IEnumerable<TestDriveVehicleModel> ListAll(string searchKeyword, int page, int pageSize)
        {
            IQueryable<TestDriveVehicleModel> result = from a in db.TestDrives.Where(x => x.Status == 1 || x.Status == 2)
                                                       join b in db.Vehicles
                                                       on a.VehicleID equals b.ID
                                                       select new TestDriveVehicleModel()
                                                       {
                                                           ID=a.ID,
                                                           VehicleID=a.VehicleID,
                                                           VehicleName=b.Name,
                                                           ModelNumber=b.ModelNumber,
                                                           CustomerID=a.CustomerID,
                                                           Email=a.Email,
                                                           Phone=a.Phone,
                                                           Address=a.Address,
                                                           DateOfBirth=a.DateOfBirth,
                                                           RegistrationDate=a.RegistrationDate,
                                                           DriverLicense=a.DriverLicense,
                                                           MoreRequest=a.MoreRequest,
                                                           Status=a.Status,
                                                       };
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                result = result.Where(x => x.Name.Contains(searchKeyword)  || x.MoreRequest.Contains(searchKeyword) || x.VehicleName.Contains(searchKeyword) || x.ModelNumber.Contains(searchKeyword));
            }          
            return result.OrderByDescending(x => x.RegistrationDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<TestDriveVehicleModel> ListConfirmTestDrive(string searchKeyword, int page, int pageSize)
        {
            IQueryable<TestDriveVehicleModel> result = from a in db.TestDrives.Where(x => x.Status == 0)
                                                       join b in db.Vehicles
                                                       on a.VehicleID equals b.ID
                                                       select new TestDriveVehicleModel()
                                                       {
                                                           ID = a.ID,
                                                           VehicleID = a.VehicleID,
                                                           VehicleName = b.Name,
                                                           ModelNumber = b.ModelNumber,
                                                           CustomerID = a.CustomerID,
                                                           Email = a.Email,
                                                           Phone = a.Phone,
                                                           Address = a.Address,
                                                           DateOfBirth = a.DateOfBirth,
                                                           RegistrationDate = a.RegistrationDate,
                                                           DriverLicense = a.DriverLicense,
                                                           MoreRequest = a.MoreRequest,
                                                           Status=a.Status,
                                                       };
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                result = result.Where(x => x.Name.Contains(searchKeyword) || x.MoreRequest.Contains(searchKeyword) || x.VehicleName.Contains(searchKeyword) || x.ModelNumber.Contains(searchKeyword));
            }
            return result.OrderByDescending(x => x.RegistrationDate).ToPagedList(page, pageSize);
        }

        public long GetIDRecent()
        {
            if (db.TestDrives.ToList().Count == 0)
            {
                return 0;
            }
            return db.TestDrives.OrderByDescending(x => x.ID).Take(1).SingleOrDefault().ID;
        }

        public TestDrive Detail(long id)
        {
            return db.TestDrives.Find(id);
        }
    }
}
