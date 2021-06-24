
namespace WebApplication.Repositories
{
    public interface IScalarFunctionsRepository
    {
        int GetNumberOfOrdersForToday();
        int GetNumberOfOrdersAllTime();
    }
}
