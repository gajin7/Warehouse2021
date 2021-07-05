using System.Linq;
using Warehouse.Model;
using Warehouse.Repository.Abstractions;

namespace Warehouse.Repository
{
    public class ScalarFunctionRepository : IScalarFunctionsRepository
    {
        public int GetNumberOfOrdersForToday()
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Database.SqlQuery<int>("SELECT dbo.NumberOfOrdersToday()").FirstOrDefault();
            }
        }

        public int GetNumberOfOrdersAllTime()
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Database.SqlQuery<int>("SELECT dbo.NumberOfOrdersAllTime()").FirstOrDefault();
            }
        }
    }
}