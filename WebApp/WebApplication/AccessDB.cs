using System.Data.Entity;


namespace WebApplication
{
    public class AccessDb : DbContext
    {
        public AccessDb() : base("WarehouseDBEntities") { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

    }
}