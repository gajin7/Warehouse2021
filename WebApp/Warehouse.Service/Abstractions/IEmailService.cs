
namespace Warehouse.Service.Abstractions
{
    public interface IEmailService
    {
        bool SendNewPasswordEmail(string emailAddress, string password);
    }
}
