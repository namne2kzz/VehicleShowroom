using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Model.EF
{
    public partial class VSMSDbContext : DbContext
    {
        public VSMSDbContext()
            : base("name=VSMSDbContext")
        {
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }      
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Dealer> Dealers { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<MandatoryCost> MandatoryCosts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceOrderDetail> ServiceOrderDetails { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<TestDrive> TestDrives { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<VehicleRegistrationData> VehicleRegistrationDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<About>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<About>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.District)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Province)
                .IsUnicode(false);

            modelBuilder.Entity<Blog>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Blog>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Brand>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Brand>()
                .Property(e => e.Image)
                .IsUnicode(false);          

            modelBuilder.Entity<Contact>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Province)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.RoleID)
                .IsUnicode(false);

            modelBuilder.Entity<Dealer>()
                .Property(e => e.DealerName)
                .IsUnicode(false);

            modelBuilder.Entity<Dealer>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Dealer>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Dealer>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Dealer>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Dealer>()
                .Property(e => e.Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<Dealer>()
                .Property(e => e.RoleID)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.RoleID)
                .IsUnicode(false);

            modelBuilder.Entity<Image>()
                .Property(e => e.Image1)
                .IsUnicode(false);

            modelBuilder.Entity<MandatoryCost>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<MandatoryCost>()
                .Property(e => e.Cost)
                .HasPrecision(18, 0);

            modelBuilder.Entity<MandatoryCost>()
                .Property(e => e.Unit)
                .IsUnicode(false);

            

            modelBuilder.Entity<Order>()
                .Property(e => e.ReceivedName)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.ReceivedAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.ReceivedMobile)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.ReceivedEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalCost)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.ReceivedName)
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.ReceivedEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.ReceivedPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Purchase>()
                .Property(e => e.TotalCost)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Role>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Role1)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Slide>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Slide>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<TestDrive>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<TestDrive>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<TestDrive>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<TestDrive>()
                .Property(e => e.DriverLicense)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.ModelNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Name)
                .IsUnicode(false);          

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Style)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Model)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Origin)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.FuelType)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Mileage)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.EngineDispl)
                .IsFixedLength();

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Transmission)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.FogLamps)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.HistoricalCost)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<VehicleRegistrationData>()
                .Property(e => e.OwnerName)
                .IsUnicode(false);

            modelBuilder.Entity<VehicleRegistrationData>()
                .Property(e => e.IdentityCardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<VehicleRegistrationData>()
                .Property(e => e.PlateNumber)
                .IsUnicode(false);

            modelBuilder.Entity<VehicleRegistrationData>()
                .Property(e => e.RegistrationPlace)
                .IsUnicode(false);

            modelBuilder.Entity<VehicleRegistrationData>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<VehicleRegistrationData>()
                .Property(e => e.Phone)
                .IsUnicode(false);
        }
    }
}
