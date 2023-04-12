using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class AboutDAO
    {
        private VSMSDbContext db;

        public AboutDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(About entity)
        {
            db.Abouts.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(About entity)
        {
            try
            {
                var about = db.Abouts.Find(entity.ID);
                about.Name = entity.Name;
                about.Email = entity.Email;
                about.Phone = entity.Phone;
                about.Address = entity.Address;
                about.Description = entity.Description;
                about.Status = entity.Status;
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
                var about = db.Abouts.Find(id);
                db.Abouts.Remove(about);
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
                var about = db.Abouts.Find(id);
                about.Status = !about.Status;
                db.SaveChanges();
                return about.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<About> ListAll(string searchKeyword, int page, int pageSize)
        {
            IQueryable<About> model = db.Abouts;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.Name.Contains(searchKeyword) || x.Description.Contains(searchKeyword));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public List<About> ListAll()
        {
            return db.Abouts.Where(x => x.Status == true).OrderByDescending(x => x.ID).ToList();
        }

        public About Detail(long id)
        {
            return db.Abouts.Find(id);
        }
    }
}
