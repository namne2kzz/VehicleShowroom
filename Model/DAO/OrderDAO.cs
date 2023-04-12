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
    public class OrderDAO
    {
        private VSMSDbContext db;
        public OrderDAO()
        {
            db = new VSMSDbContext();
        }

        public long Insert(Order entity)
        {
            db.Orders.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
       

        public bool Delete(long id)
        {
            try
            {
                var order = db.Orders.Find(id);
                db.Orders.Remove(order);
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
                var order = db.Orders.Find(id);
                order.Status = status;
                db.SaveChanges();
                return order.Status;
            }
            catch (Exception ex)
            {
                return -3;
            }
        }

        public IEnumerable<Order> ListAll(long dealerID, string searchKeyword, int page, int pageSize)
        {
            IQueryable<Order> model = db.Orders.Where(x => x.DealerID == dealerID && x.Status !=0);
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.ReceivedAddress.Contains(searchKeyword) || x.ReceivedAddress.Contains(searchKeyword) || x.ReceivedEmail.Contains(searchKeyword) || x.ReceivedMobile.Contains(searchKeyword));
            }
            
            return model.OrderByDescending(x => x.OrderedDate).ToPagedList(page, pageSize);
        }

        public IEnumerable<Order> ListConfirmBlog(long dealerID, string searchKeyword, int page, int pageSize)
        {
            IQueryable<Order> model = db.Orders.Where(x => x.DealerID == dealerID && x.Status == 0);
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                model = model.Where(x => x.ReceivedName.Contains(searchKeyword) || x.ReceivedAddress.Contains(searchKeyword) || x.ReceivedEmail.Contains(searchKeyword) || x.ReceivedMobile.Contains(searchKeyword));
            }           
            return model.OrderByDescending(x => x.OrderedDate).ToPagedList(page, pageSize);
        }      

        public Order Detail(long id)
        {
            return db.Orders.Find(id);
        }       

        public int CountVehicleByOrder(long orderId)
        {
            return db.OrderDetails.Count(x => x.OrderID == orderId);
        }

        public decimal GetCostByOrder(long orderId)
        {
            decimal cost = 0;
            var model = db.OrderDetails.Where(x => x.OrderID == orderId);
            foreach (var item in model)
            {
                var discount = new DiscountDAO().GetDiscountVehicle(item.VehicleID);
                var vehicle = new VehicleDAO().Detail(item.VehicleID);
                cost += vehicle.Price +item.RegistrationFee +item.RoadMaintenanceFee+item.CivilLiabilityInsuranceFee+item.TestingFee+item.RegistrationPlateFee - (decimal)discount*vehicle.Price;
            }
            return cost;
        }

        public bool Update(long orderId)
        {
            try
            {
                var dao = db.Orders.Find(orderId);
                dao.Quantity = new OrderDAO().CountVehicleByOrder(orderId);
                dao.TotalCost = new OrderDAO().GetCostByOrder(orderId);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<OrderDetailModel> ListAllByOrder(long orderId)
        {
            var model = (from d in db.OrderDetails
                         join a in db.Vehicles
                         on d.VehicleID equals a.ID
                         join b in db.Images
                         on a.ID equals b.VehicleID
                         where b.Status == true
                         join c in db.Discounts
                         on a.ID equals c.VehicleID
                         into temp
                         from last in temp.DefaultIfEmpty()
                         select new
                         {
                             OrderID = d.OrderID,
                             DealerID = a.DealerID,
                             VehicleID = a.ID,
                             ModelNumber=a.ModelNumber,
                             VehicleName = a.Name,                             
                             Price = a.Price,
                             RegistrationFee=d.RegistrationFee,
                             RoadMaintenanceFee=d.RoadMaintenanceFee,
                             CivilLiabilityInsurance=d.CivilLiabilityInsuranceFee,
                             TestingFee=d.TestingFee,
                             RegistrationPlateFee=d.RegistrationPlateFee,
                             SaleOff = last != null ? last.SaleOff : 0,
                             Status = a.Status,

                         }).AsEnumerable().Select(x => new OrderDetailModel()
                         {
                             OrderID=x.OrderID,
                             DealerID = x.DealerID,
                             VehicleID = x.VehicleID,
                             VehicleName = x.VehicleName,
                             ModelNumber=x.ModelNumber,
                             RegistrationFee=x.RegistrationFee,
                             RoadMaintennanceFee=x.RoadMaintenanceFee,
                             CivilLiabilityInsuranceFee=x.CivilLiabilityInsurance,
                             TestingFee=x.TestingFee,
                             RegistrationPlateFee=x.RegistrationPlateFee,
                             Price = x.Price,
                             SaleOff = x.SaleOff,                            
                             Status = x.Status,
                         });
            return model.Where(x => x.OrderID == orderId).OrderByDescending(x => x.VehicleID).ToList();
        }

        public List<Order> ListOrderByCustomer(long customerId, ref int totalRecord, int pageIndex = 1, int pageSize = 5)
        {
            totalRecord = db.Orders.Where(x => x.CustomerID == customerId).Count();

            return db.Orders.Where(x => x.CustomerID == customerId).OrderByDescending(x => x.OrderedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

    }
}
