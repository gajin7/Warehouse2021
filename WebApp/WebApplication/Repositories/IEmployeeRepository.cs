using System;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public interface IEmployeeRepository : IDisposable
    {
        Employee GetUserInfo(string email);
        Employee GetUserById(string id);
        OperationResult RegisterNewEmployee(Employee employee);
        OperationResult ChangePassword(string email, string oldPassword, string newPassword);
        LoginResult Login(string email, string password);
    }
}