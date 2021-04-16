using System.Web.Http;
using WebApplication.Controllers.Parameters;
using WebApplication.Models;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly IEmployeeRepository _userRepository;

        public UserController()
        {

        }

        public UserController(IEmployeeRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("GetUserInfo")]
        public Employee GetUserInfo([FromUri] string email)
        {
            return _userRepository.GetUserInfo(email);
        }

        [HttpPost]
        [Route("Register")]
        public OperationResult Register([FromBody] Employee employee)
        {
            return _userRepository.RegisterNewEmployee(employee);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public OperationResult ChangePassword([FromBody] ChangePasswordParameters parameters)
        {
            return _userRepository.ChangePassword(parameters.Email, parameters.OldPassword, parameters.NewPassword);
        }

        [HttpGet]
        [Route("Login")]
        public OperationResult Login([FromUri] LoginParameters parameters)
        {
            return _userRepository.Login(parameters.Email, parameters.Password);
        }

        [HttpGet]
        [Route("Logout")]
        public OperationResult Logout()
        {
            return new OperationResult()
            {
                Message = "Logout success",
                Success = true
            };
        }

    }
}