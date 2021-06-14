using System;
using System.Collections.Generic;
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

        public Employee GetUserById(string id)
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Employees.FirstOrDefault(u => u.Id.Equals(id));
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

        public OperationResult ChangeEmployeeData(Employee employee)
        {

            var result = new OperationResult();

            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (accessDb.Employees.Any(e => e.Email.Equals(employee.Email)))
                    {
                        var foundEmployee = accessDb.Employees.First(e => e.Email.Equals(employee.Email));
                        foundEmployee.FirstName = employee.FirstName;
                        foundEmployee.LastName = employee.LastName;
                        foundEmployee.Type = employee.Type;

                        var dbResult = accessDb.SaveChanges();
                        if (dbResult >= 0)
                        {
                            result.Message = "Employee data successfully changed";
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
                        result.Message = "Employee with {0} email not exist";
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
            var result = new OperationResult();
            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (accessDb.Employees.Any(e => e.Email.Equals(email)))
                    {
                        var employee = accessDb.Employees.First(u => u.Email == email);
                        if (_hashPasswordService.Verify(oldPassword,employee.Password))
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
                            result.Message = "Password is wrong. Please try again";
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

        public OperationResult ResetPassword(string email, string newPassword)
        {
            var result = new OperationResult();
            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (accessDb.Employees.Any(e => e.Email.Equals(email)))
                    {
                        var employee = accessDb.Employees.First(u => u.Email == email);
                        
                        employee.Password = _hashPasswordService.Hash(newPassword);

                        var dbResult = accessDb.SaveChanges();
                        if (dbResult > 0)
                        {
                            result.Message = "Please check you email. We just sent you new password.";
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

        public LoginResult Login(string email, string password)
        {
            var result = new LoginResult();
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
                            result.Role = employee.Type;
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

        public IEnumerable<EmployeeResult> GetAllEmployees()
        {
            using (var accessDb = new AccessDb())
            {
                var employees = accessDb.Employees.ToList();

                return employees.Select(employee => new EmployeeResult()
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = employee.Email,
                        Type = employee.Type
                })
                    .ToList();
            }
        }

        public IEnumerable<string> EmployeeTypes()
        {
            using (var accessDb = new AccessDb())
            {
                var types= accessDb.EmployeeTypes.ToList().Select(n => n.name);
                return types.ToList();
            }
        }

        public OperationResult RemoveUser(string email)
        {
            var result = new OperationResult();
            using (var accessDb = new AccessDb())
            {
                try
                {
                    if (accessDb.Employees.Any(e => e.Email.Equals(email)))
                    {
                        var employee = accessDb.Employees.First(u => u.Email == email);
                        if (employee.EmployeeType.name.Equals("admin"))
                        {
                            result.Message = "You can't remove administrator. Please change user type and then remove";
                            result.Success = false;
                        }
                        else if (employee.Warehouses.Count != 0)
                        {
                            var warehouses = "";
                            foreach (var war in employee.Warehouses)
                            {
                                warehouses = string.Join(",", war.Id);
                            }

                            result.Message = "You can't remove manager of warehouse. Please change manager of " +
                                             warehouses;
                            result.Success = false;
                        }
                        else
                        {


                            accessDb.Employees.Remove(employee);
                            var dbResult = accessDb.SaveChanges();
                            if (dbResult > 0)
                            {
                                result.Message = "Employee removed.";
                                result.Success = true;
                            }

                            else
                            {
                                result.Message = "Something went wrong. Please try again";
                                result.Success = false;
                            }
                        }
                    }

                    else
                    {
                        result.Message = "Employee with email " + email + " can't be removed";
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

        public string GetIdByEmail(string email)
        {
            using (var accessDb = new AccessDb())
            {
                var employee = accessDb.Employees.FirstOrDefault(e => e.Email.Equals(email));

                return employee?.Id;
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
    