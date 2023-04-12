using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DAO;
using Model.EF;
using Model.ViewModel;
using PagedList;

namespace Model.DAO
{
    public class BlogDAO
    {
        private VSMSDbContext db;
        public BlogDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Blog entity)
        {
            db.Blogs.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Blog entity)
        {
            try
            {
                var blog = db.Blogs.Find(entity.ID);
                blog.DealerID = entity.DealerID;
                blog.Name = entity.Name;
                blog.Description = entity.Description;             
                blog.Image = entity.Image;                                              
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
                var blog = db.Blogs.Find(id);
                db.Blogs.Remove(blog);
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
                var blog = db.Blogs.Find(id);
                blog.Status = status;
                db.SaveChanges();
                return blog.Status;
            }
            catch (Exception ex)
            {
                return -3;
            }
        }          

        public IEnumerable<Blog> ListAll(long? dealerID, string searchKeyword, int page, int pageSize)
        {
            IQueryable<Blog> model = db.Blogs;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.Name.Contains(searchKeyword) || x.Description.Contains(searchKeyword));
            }
            if (!string.IsNullOrEmpty(dealerID.ToString()))
            {
                return db.Blogs.Where(x => x.DealerID == dealerID && x.Status == 1 || x.Status == -1).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            return model.Where(x => x.Status == 1 || x.Status == -1).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Blog> ListConfirmBlog(long? dealerID, string searchKeyword, int page, int pageSize)
        {
            IQueryable<Blog> model = db.Blogs;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.Name.Contains(searchKeyword) || x.Description.Contains(searchKeyword));
            }
            if (!string.IsNullOrEmpty(dealerID.ToString()))
            {
                return db.Blogs.Where(x => x.DealerID == dealerID && x.Status == 0).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            return model.Where(x => x.Status == 0).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public long GetIDRecent()
        {
            if (db.Blogs.ToList().Count == 0)
            {
                return 0;
            }
            return db.Blogs.OrderByDescending(x => x.ID).Take(1).SingleOrDefault().ID;
        }

        public Blog Detail(long id)
        {
            return db.Blogs.Find(id);
        }

        public List<BlogDealer> ListAll()
        {
            var model = from a in db.Blogs
                        join b in db.Dealers
                        on a.DealerID equals b.ID
                        select new BlogDealer()
                        {
                            ID = a.ID,
                            DealerID = a.DealerID,
                            DealerName = b.DealerName,
                            Name = a.Name,
                            Description = a.Description,
                          
                            CreatedDate = a.CreatedDate,
                            Image = a.Image,
                            Status = a.Status,
                        };
            return model.Where(x => x.Status == 1).OrderByDescending(x => x.CreatedDate).Take(3).ToList();
        }

        public List<BlogDealer> ListAll(string searchKeyword,ref int totalRecord, int pageIndex = 1, int pageSize = 5)
        {
            totalRecord = db.Blogs.Where(x => x.Status == 1).ToList().Count();
        
        IEnumerable<BlogDealer> model = from a in db.Blogs
                        join b in db.Dealers
                        on a.DealerID equals b.ID
                        select new BlogDealer()
                        {
                            ID = a.ID,
                            DealerID = a.DealerID,
                            DealerName = b.DealerName,
                            Name = a.Name,
                            Description = a.Description,                     
                            CreatedDate = a.CreatedDate,
                            Image = a.Image,
                            Status = a.Status,
                        };
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x=>x.Status==1 && x.Name.Contains(searchKeyword) || x.Description.Contains(searchKeyword)).OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                totalRecord = db.Blogs.Where(x => x.Status == 1 && x.Name.Contains(searchKeyword) || x.Description.Contains(searchKeyword)).ToList().Count();
            }
            return model.Where(x => x.Status == 1).OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Blog> ListBlogByDealer(long dealerId)
        {
            return db.Blogs.Where(x => x.DealerID == dealerId && x.Status == 1).OrderByDescending(x => x.CreatedDate).Take(3).ToList();
        }
     
    }
}
