namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vehicle")]
    public partial class Vehicle
    {
        public long ID { get; set; }

        public long DealerID { get; set; }

        public long PurchaseID { get; set; }

        public long BrandID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage ="Model Number cannot be blank")]
        [Display(Name ="Model Number")]
        public string ModelNumber { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Vehicle Name cannot be blank")]
        [Display(Name = "Vehicle Name")]
        public string Name { get; set; }      

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Description cannot be blank")]
        public string Description { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "Detail cannot be blank")]
        public string Detail { get; set; }

        [Required(ErrorMessage = "Seat cannot be blank")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int Seat { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Style cannot be blank")]
        public string Style { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Color cannot be blank")]
        public string Color { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Model cannot be blank")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Year")]
        public string Model { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Origin cannot be blank")]
        public string Origin { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Fuel Type cannot be blank")]
        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Mileage cannot be blank")]
        public string Mileage { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage = "Engine Displacement cannot be blank")]
        [Display(Name = "Engine Displacement")]
        public string EngineDispl { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Transmission cannot be blank")]
        public string Transmission { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Foh Lamps cannot be blank")]
        public string FogLamps { get; set; }

        [Required(ErrorMessage = "Power Window cannot be blank")]
        public bool? PowerWindow { get; set; }

        [Required(ErrorMessage = "Airbags cannot be blank")]
        public bool? Airbags { get; set; }

        [Required(ErrorMessage = "ABS cannot be blank")]
        public bool? ABS { get; set; }

        [Required(ErrorMessage = "Central Locking cannot be blank")]
        [Display(Name="Central Locking")]
        public bool? CentralLocking { get; set; }

        [Required(ErrorMessage = "Historical Cost cannot be blank")]
        [Display(Name = "Historical Cost")]
        public decimal HistoricalCost { get; set; }

        [Required(ErrorMessage = "Price cannot be blank")]
        public decimal Price { get; set; }

        public long? OwnerID { get; set; }

        public bool Status { get; set; }
    }
}
