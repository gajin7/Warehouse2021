
namespace Warehouse.Repository.Abstractions
{
    public interface IScalarFunctionsRepository
    {
        int GetNumberOfOrdersForToday();
        int GetNumberOfOrdersAllTime();
    }
}
