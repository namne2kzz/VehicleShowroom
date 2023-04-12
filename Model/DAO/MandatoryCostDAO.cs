using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class MandatoryCostDAO
    {
        private VSMSDbContext db;

        public MandatoryCostDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(MandatoryCost entity)
        {
            db.MandatoryCosts.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(MandatoryCost entity)
        {
            try
            {
                var mandatoryCost = db.MandatoryCosts.Find(entity.ID);
                mandatoryCost.Name = entity.Name;
                mandatoryCost.Description = entity.Description;
                mandatoryCost.Cost = entity.Cost;
                mandatoryCost.Unit = entity.Unit;
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
                var mandatoryCost = db.MandatoryCosts.Find(id);
                db.MandatoryCosts.Remove(mandatoryCost);
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
                var mandatoryCost = db.MandatoryCosts.Find(id);
                mandatoryCost.Status = !mandatoryCost.Status;
                db.SaveChanges();
                return mandatoryCost.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<MandatoryCost> ListAll(string searchKeyword, int page, int pageSize)
        {
            IQueryable<MandatoryCost> model = db.MandatoryCosts;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.Name.Contains(searchKeyword) || x.Description.Contains(searchKeyword));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public List<MandatoryCost> ListAll()
        {
            return db.MandatoryCosts.Where(x => x.Status == true).OrderBy(x => x.ID).ToList();
        }

        public MandatoryCost Detail(long id)
        {
            return db.MandatoryCosts.Find(id);
        }
    }
}
