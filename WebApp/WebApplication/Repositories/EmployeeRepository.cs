using System;
using System.Linq;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AccessDb _accessDb;
        private readonly IHashPasswordService _hashPasswordService;

        public EmployeeRepository(IHashPasswordService hashPasswordService, AccessDb accessDb)
        {
            _hashPasswordService = hashPasswordService;
            _accessDb = accessDb;
        }

        public Employee GetUserInfo(string email)
        {
            return _accessDb.Employees.FirstOrDefault(u => u.Email.Equals(email));

        }

        public OperationResult RegisterNewEmployee(Employee employee)
        {
            employee.Password = _hashPasswordService.Hash(employee.Password);
            employee.Id = Guid.NewGuid().ToString();
            var result = new OperationResult();

            try
            {
                if (!_accessDb.Employees.Any(e => e.Email.Equals(employee.Email)))
                {
                    _accessDb.Employees.Add(employee);
                    var dbResult = _accessDb.SaveChanges();
                    if (dbResult > 0)
                    {
                        result.Message = "Employee registered successfully";
                        result.Success = true;
                    }
                    else
                    {
                        result.Message = "Something went wrong. Please try again";
                        result.Success = false;
                    }
                }
                else
                {
                    result.Message = "Employee with same email already exist";
                    result.Success = false;
                }
            }
            catch (Exception e)
            {
                result.Message = "An error occurred. Check your data and try again";
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            return result;
        }


        public OperationResult ChangePassword(string email, string oldPassword, string newPassword)
        {
            var oldPasswordHash = _hashPasswordService.Hash(oldPassword);
            var result = new OperationResult();
            try
            {
                if (_accessDb.Employees.Any(e => e.Email.Equals(email)))
                {
                    var employee = _accessDb.Employees.First(u => u.Email == email);
                    if (employee.Password.Equals(oldPasswordHash))
                    {
                        employee.Password = _hashPasswordService.Hash(newPassword);

                        var dbResult = _accessDb.SaveChanges();
                        if (dbResult > 0)
                        {
                            result.Message = "Password changed successfully";
                            result.Success = true;
                        }
                    }
                    else
                    {
                        result.Message = "Something went wrong. Please try again";
                        result.Success = false;
                    }
                }
                else
                {
                    result.Message = "Employee with email " + email + " doesn't exist";
                    result.Success = false;
                }
            }
            catch (Exception e)
            {
                result.Message = "An error occurred. Check your data and try again";
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            return result;
        }

        public OperationResult Login(string email, string password)
        {
            var passwordHash = _hashPasswordService.Hash(password);
            var result = new OperationResult();
            try
            {
                if (_accessDb.Employees.Any(e => e.Email.Equals(email)))
                {
                    var employee = _accessDb.Employees.First(u => u.Email == email);
                    if (employee.Password.Equals(passwordHash))
                    {
                        result.Message = "Success";
                        result.Success = true;
                        
                    }
                    else
                    {
                        result.Message = "Please check your password";
                        result.Success = false;
                    }
                }
                else
                {
                    result.Message = "Employee with email " + email + " doesn't exist";
                    result.Success = false;
                }
            }
            catch (Exception e)
            {
                result.Message = "An error occurred. Check your data and try again";
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            return result;
        }
    }
}
    