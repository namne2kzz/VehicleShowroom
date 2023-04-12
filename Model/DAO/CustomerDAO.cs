using Common;
using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class CustomerDAO
    {
        private VSMSDbContext db;

        public CustomerDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Customer entity)
        {
            try
            {
                db.Customers.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch(System.Data.Entity.Validation.DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;               
            }
            
        }

        public bool Update(Customer entity)
        {
            try
            {
                var customer = db.Customers.Find(entity.ID);
                customer.UserName = entity.UserName;
                customer.Password = entity.Password;
                customer.Name = entity.Name;
                customer.DateOfBirth = entity.DateOfBirth;
                customer.Email = entity.Email;
                customer.Phone = entity.Phone;
                customer.Avatar = entity.Avatar;
                customer.RoleID = entity.RoleID;
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
                var customer = db.Customers.Find(id);
                db.Customers.Remove(customer);
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
                var customer = db.Customers.Find(id);
                customer.Status = !customer.Status;
                db.SaveChanges();
                return customer.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckUserNameExist(string userName)
        {
            return db.Customers.Count(x => x.UserName == userName) > 0;
        }

        public bool CheckEmailExist(string email)
        {
            return db.Customers.Count(x => x.Email == email) > 0;
        }

        public IEnumerable<Customer> ListAll(string searchKeyword, int page, int pageSize)
        {
            IQueryable<Customer> model = db.Customers;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.UserName.Contains(searchKeyword) || x.Name.Contains(searchKeyword));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public Customer GetCustomerByUserName(string userName)
        {
            return db.Customers.SingleOrDefault(x => x.UserName == userName);
        }

        public Customer Detail(long id)
        {
            return db.Customers.Find(id);
        }

        public int Login(string userName, string passWord, bool isLoginCustomer = false)
        {
            var result = db.Customers.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginCustomer == true)
                {
                    if (result.RoleID == Constants.CUSTOMER_GROUP)
                    {
                        if (result.Status == false)
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
                    if (result.Status == false)
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
            if (db.Customers.ToList().Count == 0)
            {
                return 0;
            }
            return db.Customers.OrderByDescending(x => x.ID).Take(1).SingleOrDefault().ID;
        }

        public Customer GetCustomerByEmail(string email)
        {
            return db.Customers.SingleOrDefault(x => x.Email == email);
        }

        public Customer CheckCustomer(string userName, string password)
        {
            return db.Customers.SingleOrDefault(x => x.UserName == userName && x.Password == password);
        }

        public List<int> ListCountCustomerByStatus()
        {
            var result = new List<int>();
            var active = db.Customers.Count(x => x.Status == true); 
            var locked = db.Customers.Count(x => x.Status == false);
            result.Add(active);
            result.Add(locked);
            return result;
        }
       
    }
}
