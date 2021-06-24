using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using WebApplication.Models;

namespace WebApplication.Repositories
{
    public class ReportRepository : IReportRepository
    {
        public OperationResult CreateInReport(Item item)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var report = accessDb.Reports.ToList()
                        .FirstOrDefault(r => r.Date.Equals(DateTime.Now.ToShortDateString()) && r.Type == "In");
                    if (report != null)
                    {
                        var reportItem = new ItemReport
                        {
                            ItemId = item.Id,
                            ReportId = report.Id,
                            Quantity = item.Quantity
                        };
                        report.ItemReports.Add(reportItem);
                    }
                    else
                    {
                        report = new Report()
                        {
                            Date = DateTime.Now.ToShortDateString(),
                            Type = "In",
                            Id = Guid.NewGuid().ToString()
                        };
                        var reportItem = new ItemReport()
                        {
                            ItemId = item.Id,
                            ReportId = report.Id,
                            Quantity = item.Quantity
                        };
                        report.ItemReports.Add(reportItem);

                        accessDb.Reports.Add(report);
                    }

                    var result = accessDb.SaveChanges();

                        if (result > 0)
                        {
                            return new OperationResult()
                            {
                                Message = "Report created successfully",
                                Success = true
                            };
                        }
                        else
                        {
                            return new OperationResult()
                            {
                                Message = "Report can't be created at the moment, please try again",
                                Success = false
                            };
                        }
                }
                
                catch (Exception e)
                {
                    return new OperationResult()
                    {
                        Message = "Report can't be created at the moment, please try again",
                        Success = false,
                        ErrorMessage = e.Message
                    };
                }
            }
        }

        public ReportOperationResult CreateOutReport(IEnumerable<ItemResult> items)
        {
            using (var accessDb = new AccessDb())
            {
                try
                {
                    var enumerable = items.ToList();
                    var report = new Report()
                    {
                        Date = DateTime.Now.ToShortDateString(),
                        Type = "Out",
                        Id = Guid.NewGuid().ToString()
                    };
                    var reportItems = enumerable.ToList().Select(item => new ItemReport()
                        { ItemId = item.Id, ReportId = report.Id, Quantity = item.Quantity}).ToList();
                    report.ItemReports = reportItems;
                    accessDb.Reports.Add(report);
                    var result = accessDb.SaveChanges();

                    if (result > 0)
                    {
                        return new ReportOperationResult()
                        {
                            Message = "Report created successfully",
                            Success = true,
                            ReportId = report.Id
                        };
                    }
                    else
                    {
                        return new ReportOperationResult()
                        {
                            Message = "Report can't be created at the moment, please try again",
                            Success = false
                        };

                    }
                }
                catch (Exception e)
                {
                    return new ReportOperationResult()
                    {
                        Message = "Receipt can't be created at the moment, please try again",
                        Success = false,
                        ErrorMessage = e.Message
                    };
                }
            }

        }

        public Report GetReport(string reportId)
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.Reports.FirstOrDefault(r => r.Id.Equals(reportId));
            }
        }

        public IEnumerable<ItemReport> GetReportItems(string reportId)
        {
            using (var accessDb = new AccessDb())
            {
                return accessDb.ItemReports.Where(ri => ri.ReportId.Equals(reportId)).ToList();
            }
        }

        public IEnumerable<ReportResult> GetReports(string reportType)
        {
            using (var accessDb = new AccessDb())
            {
                var reports = reportType.IsNullOrWhiteSpace() ? accessDb.Reports.ToList() : accessDb.Reports.Where(r => r.Type.Equals(reportType)).ToList();

                return reports.Select(report => new ReportResult() {Date = report.Date, Id = report.Id, Type = report.Type}).ToList();
            }
        }
    }
}