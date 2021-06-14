
namespace WebApplication.Services
{
    public interface IEmailService
    {
        bool SendNewPasswordEmail(string emailAddress, string password);
    }
}
