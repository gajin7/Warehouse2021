using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
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
        [Route("GetUserInfo")]
        public Employee GetUserInfo([FromUri] string email)
        {
            return _userRepository.GetUserInfo(email);
        }
    }
}