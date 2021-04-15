using System.Data.Entity;


namespace WebApplication
{
    public class AccessDB : DbContext
    {
        public AccessDB() : base("WarehouseDBEntities") { }

        public DbSet<Employee> employees { get; set; }
        
    }
}