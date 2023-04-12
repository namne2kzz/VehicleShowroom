using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;
using PagedList;

namespace Model.DAO
{
    public class ContactDAO
    {
        private VSMSDbContext db;

        public ContactDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Contact entity)
        {
            try
            {
                db.Contacts.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch(Exception ex)
            {
                return -3;
            }
          
        }

        public bool Update(Contact entity)
        {
            try
            {
                var contact = db.Contacts.Find(entity.ID);
                contact.Name = entity.Name;
                contact.Email = entity.Email;
                contact.Phone = entity.Phone;
                contact.Content = entity.Content;
                contact.Province = entity.Province;
                contact.CreatedDate = entity.CreatedDate;
                contact.Status = entity.Status;
                db.Contacts.Add(contact);
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
                var contact = db.Contacts.Find(id);
                db.Contacts.Remove(contact);
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
                var contact = db.Contacts.Find(id);
                contact.Status = !contact.Status;
                db.SaveChanges();
                return contact.Status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Contact> ListAll(string searchKeyword, int page, int pageSize)
        {
            IQueryable<Contact> model = db.Contacts;
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.Name.Contains(searchKeyword) || x.Content.Contains(searchKeyword) || x.Email.Contains(searchKeyword) || x.Phone.Contains(searchKeyword));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

        public List<Contact> ListAll()
        {
            return db.Contacts.Where(x => x.Status == true).OrderByDescending(x => x.ID).ToList();
        }

        public Contact Detail(long id)
        {
            return db.Contacts.Find(id);
        }

        public IEnumerable<AllotmentContactReport> ListCountContactByProvince(int month)
        {         
            IQueryable<AllotmentContactReport> result = db.Contacts.Where(x=>x.CreatedDate.Month==month).GroupBy(x => x.Province).Select(x => new AllotmentContactReport()
            {
                Province = x.Key,
                Quantity = x.Count()
            });
            return result;
        }
    }
}
