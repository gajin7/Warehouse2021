
namespace Warehouse.Service.Abstractions
{
    public interface IHashPasswordService
    {
        string Hash(string password);
        bool Verify(string password, string hashedPassword);
    }
}