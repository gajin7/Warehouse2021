using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public Employee GetUserInfo(string email)
        {
            using (var access = new AccessDB())
            {
                return access.employees.Where(u => u.employeeEmail.Equals(email)).FirstOrDefault();
            }
        }
    }
}