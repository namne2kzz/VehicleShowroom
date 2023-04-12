using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class SlideDAO
    {

        private VSMSDbContext db;

        public SlideDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Slide entity)
        {
            db.Slides.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Slide entity)
        {
            try
            {
                var slide = db.Slides.Find(entity.ID);
                slide.Title = entity.Title;             
                slide.Image = entity.Image;
                slide.Description = entity.Description;
                slide.Detail = entity.Detail;
                slide.Type = entity.Type;
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
                var slide = db.Slides.Find(id);
                db.Slides.Remove(slide);
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
                var slide = db.Slides.Find(id);
                slide.Status = !slide.Status;
                db.SaveChanges();
                return slide.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Slide> ListAll(string searchKeyword, int page, int pageSize)
        {
            IQueryable<Slide> model = db.Slides;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.Title.Contains(searchKeyword) || x.Description.Contains(searchKeyword));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderByDescending(x => x.ID).ToList();
        }

        public List<Slide> MainSlide()
        {
            return db.Slides.Where(x => x.Type.Equals("MAIN") && x.Status == true).OrderByDescending(x => x.ID).ToList();
        }

        public List<Slide> ExtraSlide()
        {
            return db.Slides.Where(x => x.Type.Equals("EXTRA") && x.Status == true).OrderByDescending(x => x.ID).ToList();
        }

        public Slide Detail(long id)
        {
            return db.Slides.Find(id);
        }

        public long GetIDRecent()
        {
            if (db.Slides.ToList().Count == 0)
            {
                return 0;
            }
            return db.Slides.OrderByDescending(x => x.ID).Take(1).SingleOrDefault().ID;
        }
    }
}
