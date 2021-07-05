using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Warehouse.Model;
using Warehouse.Repository.Abstractions;
using Warehouse.Service.Abstractions;
using Warehouse.Web.Model.Request;
using Warehouse.Web.Model.Response;


namespace WebApplication.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly IEmployeeRepository _userRepository;
        private readonly ISearchService _searchService;
        private readonly IEmailService _emailService;
        private readonly IDefaultPasswordGenerator _defaultPasswordGenerator;

        public UserController()
        {

        }

        public UserController(IEmployeeRepository userRepository, ISearchService searchService,
            IEmailService emailService, IDefaultPasswordGenerator defaultPasswordGenerator)
        {
            _userRepository = userRepository;
            _searchService = searchService;
            _emailService = emailService;
            _defaultPasswordGenerator = defaultPasswordGenerator;
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserInfo")]
        public EmployeeResult GetUserInfo([FromUri] string email)
        {
            var employee = _userRepository.GetUserInfo(email);
            return  new EmployeeResult
            {
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Type = employee.Type
            };
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("Register")]
        public OperationResult Register([FromBody] Employee employee)
        {
            return _userRepository.RegisterNewEmployee(employee);
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("DeleteEmployee")]
        public OperationResult DeleteEmployee([FromUri] string email)
        {
            return _userRepository.RemoveUser(email);
        }

        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public OperationResult ChangePassword([FromBody] ChangePasswordParameters parameters)
        {
            if (parameters == null || parameters.Email.IsNullOrWhiteSpace() ||
                parameters.OldPassword.IsNullOrWhiteSpace() || parameters.NewPassword.IsNullOrWhiteSpace())
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Please check your data and try again!"
                };

            }
            return _userRepository.ChangePassword(parameters.Email, parameters.OldPassword, parameters.NewPassword);
        }

        [HttpGet]
        [Route("resetPassword")]
        public OperationResult ResetPassword([FromUri] string email)
        {
            var pass = _defaultPasswordGenerator.GetDefaultPassword();
            var result = _userRepository.ResetPassword(email, pass);
            if (result.Success)
            {
                var emailResult = _emailService.SendNewPasswordEmail(email, pass);
                if (emailResult)
                {
                    return result;
                }
                else
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Email can't be sent at the moment. Please try again"
                    };
                }
            }
            else
            {
                return result;
            }
        }

        [HttpPost]
        [Authorize]
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

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("GetAllUsers")]
        public IEnumerable<EmployeeResult> GetAllEmployees()
        {
            return _userRepository.GetAllEmployees();
        }

        [HttpGet]
        [Authorize]
        [Route("GetEmployeeTypes")]
        public IEnumerable<string> GetEmployeeTypes()
        {
            return _userRepository.EmployeeTypes();
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("ChangeEmployeeData")]
        public OperationResult ChangeEmployeeData(Employee employee)
        {
            return _userRepository.ChangeEmployeeData(employee);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("SearchEmployees")]
        public IEnumerable<EmployeeResult> SearchEmployees([FromUri]string keyWord)
        {
            var employees = _userRepository.GetAllEmployees();
            return _searchService.FilterAllEmployeesBaseOnKeyWord(employees, keyWord);
        }
    }
}