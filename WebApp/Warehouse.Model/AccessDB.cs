using System.Data.Entity;


namespace Warehouse.Model
{
    public class AccessDb : DbContext
    {
        public AccessDb() : base("WarehouseDBEntities") { }
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public  DbSet<Report> Reports { get; set; }
        public DbSet<Load> Loads { get; set; }
        public DbSet<Ramp> Ramps { get; set; }
        public DbSet<ItemReport> ItemReports { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }
        public DbSet<Pricelist> Pricelists { get; set; }

    }
}