
namespace WebApplication.Controllers.Parameters
{
    public class ChangePasswordParameters
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}