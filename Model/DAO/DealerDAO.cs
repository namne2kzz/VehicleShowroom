using Common;
using Model.EF;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class DealerDAO
    {
        private VSMSDbContext db;
        public DealerDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Dealer entity)
        {
            try
            {
                db.Dealers.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                return -1;
            }
           
        }

        public bool Update(Dealer entity)
        {
            try
            {
                var dealer = db.Dealers.Find(entity.ID);
                dealer.UserName = entity.UserName;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    dealer.Password = entity.Password;
                }
                dealer.DealerName = entity.DealerName;              
                dealer.Email = entity.Email;
                dealer.Phone = entity.Phone;               
                dealer.Avatar = entity.Avatar;
                dealer.RoleID = entity.RoleID;                        
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
                var dealer = db.Dealers.Find(id);
                db.Dealers.Remove(dealer);
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
                var dealer = db.Dealers.Find(id);
                dealer.Status = status;
                db.SaveChanges();
                return dealer.Status;
            }
            catch (Exception ex)
            {
                return -3;
            }
        }


        public bool CheckUserNameExist(string userName)
        {
            return db.Dealers.Count(x => x.UserName == userName) > 0;
        }

        public bool CheckEmailExist(string email)
        {
            return db.Dealers.Count(x => x.Email == email) > 0;
        }

        public bool CheckDealerNameExist(string dealerName)
        {
            return db.Dealers.Count(x => x.DealerName == dealerName) > 0;
        }

        public IEnumerable<Dealer> ListAll(string searchKeyword, int page, int pageSize)
        {
            IQueryable<Dealer> model = db.Dealers;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.UserName.Contains(searchKeyword) || x.DealerName.Contains(searchKeyword));
            }
            return model.Where(x=>x.Status == 1 || x.Status == -1).OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public IEnumerable<Dealer> ListConfirmDealer(string searchKeyword, int page, int pageSize)
        {
            IQueryable<Dealer> model = db.Dealers;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.UserName.Contains(searchKeyword) || x.DealerName.Contains(searchKeyword));
            }
            return model.Where(x=>x.Status==0).OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }



        public Dealer GetByUserName(string userName)
        {
            return db.Dealers.SingleOrDefault(x => x.UserName == userName);
        }

        public Dealer Detail(long id)
        {
            return db.Dealers.Find(id);
        }

        public int Login(string userName, string passWord, bool isLoginDealer = false)
        {
            var result = db.Dealers.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginDealer == true)
                {
                    if (result.RoleID == Constants.DEALER_GROUP)
                    {
                        if (result.Status == 0 || result.Status == -1)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == 0 || result.Status == -1)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
                }
            }
        }       

        public long GetIDRecent()
        {
            if (db.Dealers.ToList().Count == 0)
            {
                return 0;
            }
            return db.Dealers.OrderByDescending(x => x.ID).Take(1).SingleOrDefault().ID;
        }

        public IEnumerable<Dealer> ListAll()
        {
            IEnumerable<Dealer> model= db.Dealers.Where(x => x.Status == 1 || x.Status == -1).OrderByDescending(x => x.ID).ToList();
            return model;
        }

        public Dealer GetDealerByEmail(string email)
        {
            return db.Dealers.SingleOrDefault(x => x.Email == email);
        }

        public Dealer CheckDealer(string userName, string password)
        {
            return db.Dealers.SingleOrDefault(x => x.UserName == userName && x.Password == password);
        }

        public List<DealerVehicleReport> ReportDealerVehicleSell(int month)
        {
            var dealer = db.Dealers.Where(x => x.Status == 1).ToList();
            var order = db.Orders.Where(x => x.Status == 3).ToList();
            var result = new List<DealerVehicleReport>();
            foreach (var item in dealer)
            {
                var subList = new DealerVehicleReport();
                int qty = 0;
                foreach (var subItem in order)
                {
                    if (item.ID == subItem.DealerID && subItem.OrderedDate.Month==month)
                    {
                        qty += subItem.Quantity;
                    }                   
                }
                subList.DealerName = item.DealerName;
                subList.Quantity = qty;
                result.Add(subList);
            }
            return result;
        }

        public List<int> ListCountDealerByStatus()
        {
            var result = new List<int>();
            var active = db.Dealers.Count(x => x.Status == 1);
            var locked = db.Dealers.Count(x => x.Status == -1);
            var waiting = db.Dealers.Count(x => x.Status == 0);
            result.Add(active);
            result.Add(locked);
            result.Add(waiting);
            return result;
        }
    }
}
