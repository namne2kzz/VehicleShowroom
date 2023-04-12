using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DAO;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class ServiceDAO
    {
        private VSMSDbContext db;

        public ServiceDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Service entity)
        {
            db.Services.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Service entity)
        {
            try
            {
                var service = db.Services.Find(entity.ID);
                service.DealerID = entity.DealerID;
                service.Name = entity.Name;
                service.Description = entity.Description;
                service.Status = entity.Status;
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
                var service = db.Services.Find(id);
                db.Services.Remove(service);
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
                var service = db.Services.Find(id);
                service.Status = !service.Status;
                db.SaveChanges();
                return service.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Service> ListAll(long dealerId,string searchKeyword, int page, int pageSize)
        {
            IQueryable<Service> model = db.Services.Where(x=>x.DealerID==dealerId);
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.Name.Contains(searchKeyword) || x.Description.Contains(searchKeyword));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public List<Service> ListAll(long dealerId)
        {
            return db.Services.Where(x => x.Status == true && x.DealerID==dealerId).OrderByDescending(x => x.ID).ToList();
        }

        public Service Detail(long id)
        {
            return db.Services.Find(id);
        }
    }
}
