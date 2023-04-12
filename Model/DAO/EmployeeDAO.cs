using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Model.DAO
{
    public class EmployeeDAO
    {
        private VSMSDbContext db;

        public EmployeeDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Employee entity)
        {
            try
            {
                db.Employees.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch(System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                return -1;
            }
          
        }

        public bool Update(Employee entity)
        {
            try
            {
                var employee = db.Employees.Find(entity.ID);
                employee.UserName = entity.UserName;                
                employee.Password = entity.Password;               
                employee.Name = entity.Name;
                employee.DateOfBirth = entity.DateOfBirth;
                employee.Email = entity.Email;
                employee.Phone = entity.Phone;
                employee.Address = entity.Address;
                employee.Avatar = entity.Avatar;
                employee.RoleID = entity.RoleID;            
                employee.Status = entity.Status;
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
                var employee = db.Employees.Find(id);
                db.Employees.Remove(employee);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            try
            {
                var employee = db.Employees.Find(id);
                employee.Status = !employee.Status;
                db.SaveChanges();
                return employee.Status;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool CheckUserNameExist(string userName)
        {
            return db.Employees.Count(x => x.UserName == userName) > 0;
        }

        public bool CheckEmailExist(string email)
        {
            return db.Employees.Count(x => x.Email == email) > 0;
        }

        public IEnumerable<Employee> ListAll(string searchKeyword, int page, int pageSize)
        {
            IQueryable<Employee> model = db.Employees;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.UserName.Contains(searchKeyword) || x.Name.Contains(searchKeyword));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public Employee GetByUserName(string userName)
        {
            return db.Employees.SingleOrDefault(x => x.UserName == userName);
        }

        public Employee Detail(long id)
        {
            return db.Employees.Find(id);
        }

        public int Login(string userName, string passWord, bool isLoginShowroom = false)
        {
            var result = db.Employees.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginShowroom == true)
                {
                    if (result.RoleID == Constants.SHOWROOM_GROUP )
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
            if (db.Employees.ToList().Count == 0)
            {
                return 0;
            }
            return db.Employees.OrderByDescending(x => x.ID).Take(1).SingleOrDefault().ID;
        }

        public Employee GetEmployeeByEmail(string email)
        {
            return db.Employees.SingleOrDefault(x => x.Email == email);
        }

        public Employee CheckEmployee(string userName,string password)
        {
            return db.Employees.SingleOrDefault(x => x.UserName == userName && x.Password == password);
        }
    }
}
