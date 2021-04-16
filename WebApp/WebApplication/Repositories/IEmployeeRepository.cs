using System;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface IEmployeeRepository : IDisposable
    {
        Employee GetUserInfo(string email);
        OperationResult RegisterNewEmployee(Employee employee);
        OperationResult ChangePassword(string email, string oldPassword, string newPassword);
        OperationResult Login(string email, string password);
    }
}