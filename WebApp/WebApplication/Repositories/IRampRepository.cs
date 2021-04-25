
namespace WebApplication.Repositories
{
    public interface IRampRepository
    {
        Ramp GetRampForLoad(string warehouseId);
    }
}