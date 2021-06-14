using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class CompaniesRepository : ICompaniesRepository
    {
        public IEnumerable<Company> GetCompanies()
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Companies.ToList();
            }
        }

        public OperationResult AddCompany(CompanyResult company)
        {
            if (!AssertCompany(company))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Please check your data and try again"
                };
            }
            using (var accessDb = new AccessDb())
            {
                try
                {
                    if(accessDb.Companies.Any(c => c.PIB.Equals(company.PIB)))
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Company with same PIB already exist. Please try again"
                        };
                    }

                    var depositValid = bool.TryParse(company.Deposit, out var deposit);
                    accessDb.Companies.Add(new Company
                    {
                        Name = company.Name,
                        AccountNo = company.AccountNo,
                        Address = company.Address,
                        Deposit = depositValid && deposit,
                        PIB = company.PIB
                    });
                    var result = accessDb.SaveChanges();
                    if (result > 0)
                    {
                        return new OperationResult
                        {
                            Success = true,
                            Message = "Company successfully added"
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Something went wrong. Please try again"
                        };
                    }

                }

                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong. Please try again",
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public OperationResult ChangeCompany(CompanyResult company)
        {
            if (!AssertCompany(company))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Please check your data and try again"
                };
            }
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var cmp = accessDb.Companies.FirstOrDefault(c => c.PIB.Equals(company.PIB));
                    if (cmp == null)
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Company can't be found. Please try again"
                        };
                    }
                    var depositValid = bool.TryParse(company.Deposit, out var deposit);
                    cmp.AccountNo = company.AccountNo;
                    cmp.Address = company.Address;
                    cmp.Name = company.Name;
                    cmp.Deposit = depositValid && deposit;

                    var result = accessDb.SaveChanges();
                    if (result >= 0)
                    {
                        return new OperationResult
                        {
                            Success = true,
                            Message = "Company successfully changed"
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Something went wrong. Please try again"
                        };
                    }

                }

                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong. Please try again",
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public OperationResult RemoveCompany(string companyPib)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var cmp = accessDb.Companies.FirstOrDefault(c => c.PIB.Equals(companyPib));
                    if (cmp == null)
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Company can't be found"
                        };
                    }
                   
                    accessDb.Companies.Remove(cmp);
                    var result = accessDb.SaveChanges();

                    if (result > 0)
                    {
                        return new OperationResult
                        {
                            Success = true,
                            Message = "Company successfully removed"
                        };
                    }
                    else
                    {
                        return new OperationResult
                        {
                            Success = false,
                            Message = "Something went wrong. Please try again",
                        };
                    }
                }
                catch (Exception e)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Something went wrong. Please try again",
                        ErrorMessage = e.Message
                    };
                }
            }

        }

        public bool AssertCompany(CompanyResult company)
        {
            return !company.AccountNo.IsNullOrWhiteSpace() && !company.Address.IsNullOrWhiteSpace() 
                                                           && company.Deposit != null && !company.Name.IsNullOrWhiteSpace() 
                                                           && !company.PIB.IsNullOrWhiteSpace();
        }
    }
}