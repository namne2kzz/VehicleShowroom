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
    public class PurchaseDAO
    {
        private VSMSDbContext db;
        public PurchaseDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Purchase entity)
        {
            db.Purchases.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Purchase entity)
        {
            try
            {
                var purchase = db.Purchases.Find(entity.ID);
                purchase.DealerID = entity.DealerID;
                purchase.ReceivedName = entity.ReceivedName;
                purchase.ReceivedAddress = entity.ReceivedAddress;
                purchase.ReceivedEmail = entity.ReceivedEmail;
                purchase.ReceivedPhone = entity.ReceivedPhone;
                purchase.CreatedDate = entity.CreatedDate;
                purchase.ReceivedDate = entity.ReceivedDate;
                purchase.Quantity = entity.Quantity;
                purchase.TotalCost = entity.TotalCost;              
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
                var purchase = db.Purchases.Find(id);
                db.Purchases.Remove(purchase);
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
                var purchase = db.Purchases.Find(id);
                purchase.Status = status;
                db.SaveChanges();
                return purchase.Status;
            }
            catch (Exception ex)
            {
                return -3;
            }
        }

        public IEnumerable<Purchase> ListAll(long dealerId, string searchKeyword, int page, int pageSize)
        {
            IQueryable<Purchase> model = db.Purchases.Where(x=>x.DealerID==dealerId);
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.ReceivedName.Contains(searchKeyword) || x.ReceivedAddress.Contains(searchKeyword) || x.CreatedDate.ToString().Contains(searchKeyword) || x.Quantity.ToString().Contains(searchKeyword) || x.TotalCost.ToString().Contains(searchKeyword));
            }
            return model.Where(x => x.Status == 1 ).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Purchase> ListWaiting(long dealerId, string searchKeyword, int page, int pageSize)
        {
            IQueryable<Purchase> model = db.Purchases.Where(x=>x.DealerID==dealerId);
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.ReceivedName.Contains(searchKeyword) || x.ReceivedAddress.Contains(searchKeyword) || x.CreatedDate.ToString().Contains(searchKeyword) || x.Quantity.ToString().Contains(searchKeyword) || x.TotalCost.ToString().Contains(searchKeyword));
            }

            return model.Where(x => x.Status == 0).OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public List<Purchase> ListWaiting(long dealerId)
        {
            return db.Purchases.Where(x => x.Status == 0 && x.DealerID==dealerId).OrderByDescending(x => x.CreatedDate).ToList();
        }
       
        public Purchase Detail(long id)
        {
            return db.Purchases.Find(id);
        }

        public bool Update(long id, decimal cost)
        {
            try
            {
                var dao = db.Purchases.Find(id);
                dao.Quantity += 1;
                dao.TotalCost += cost;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Update(long id, DateTime receivedDate)
        {
            try
            {
                var dao = db.Purchases.Find(id);
                dao.ReceivedDate = receivedDate;
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
