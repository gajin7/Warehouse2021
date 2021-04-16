using System.Data.Entity;


namespace WebApplication
{
    public class AccessDb : DbContext
    {
        public AccessDb() : base("WarehouseDBEntities") { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Item> Items { get; set; }

    }
}