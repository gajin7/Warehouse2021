using System;
using System.Linq;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IHashPasswordService _hashPasswordService;

        public EmployeeRepository(IHashPasswordService hashPasswordService)
        {
            _hashPasswordService = hashPasswordService;
        }

        public Employee GetUserInfo(string email)
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Employees.FirstOrDefault(u => u.Email.Equals(email));
            }

        }

        public OperationResult RegisterNewEmployee(Employee employee)
        {
            employee.Password = _hashPasswordService.Hash(employee.Password);
            employee.Id = Guid.NewGuid().ToString();
            var result = new OperationResult();

            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (!accessDb.Employees.Any(e => e.Email.Equals(employee.Email)))
                    {
                        accessDb.Employees.Add(employee);
                        var dbResult = accessDb.SaveChanges();
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
        }


        public OperationResult ChangePassword(string email, string oldPassword, string newPassword)
        {
            var oldPasswordHash = _hashPasswordService.Hash(oldPassword);
            var result = new OperationResult();
            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (accessDb.Employees.Any(e => e.Email.Equals(email)))
                    {
                        var employee = accessDb.Employees.First(u => u.Email == email);
                        if (employee.Password.Equals(oldPasswordHash))
                        {
                            employee.Password = _hashPasswordService.Hash(newPassword);

                            var dbResult = accessDb.SaveChanges();
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
            }

            return result;
        }

        public OperationResult Login(string email, string password)
        {
            var result = new OperationResult();
            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (accessDb.Employees.Any(e => e.Email.Equals(email)))
                    {
                        var employee = accessDb.Employees.First(u => u.Email == email);
                        if (_hashPasswordService.Verify(password,employee.Password))
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

        public void Dispose()
        {
            using (var accessDb = new AccessDb())
            {
                accessDb.Dispose();
            }
        }
    }
}
    