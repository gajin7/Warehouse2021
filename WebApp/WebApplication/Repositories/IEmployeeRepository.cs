using System;
using System.Collections.Generic;
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
        IEnumerable<EmployeeResult> GetAllEmployees();
        IEnumerable<string> EmployeeTypes();
        OperationResult ChangeEmployeeData(Employee employee);
        OperationResult RemoveUser(string email);
        OperationResult ResetPassword(string email, string newPassword);
        string GetIdByEmail(string email);
    }
}