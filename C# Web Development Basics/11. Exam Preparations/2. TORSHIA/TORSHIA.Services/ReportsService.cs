using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TORSHIA.Data;
using TORSHIA.Models;
using TORSHIA.Models.Enums;
using TORSHIA.Services.Base;
using TORSHIA.Services.Contracts;
using TORSHIA.ViewModels.Reports;

namespace TORSHIA.Services
{
    public class ReportsService : BaseService, IReportsService
    {
        public ReportsService(TorshiaDbContext context)
            : base(context)
        {
        }

        public void CreateReport(int taskId, string username)
        {
            int reporterId = this.context.Users
                .FirstOrDefault(u => u.Username == username)
                .Id;

            var report = new Report
            {
                Status = new Random().Next(1, 5) == 4 ? Status.Archived : Status.Completed,
                ReportedOn = DateTime.UtcNow,
                TaskId = taskId,
                ReporterId = reporterId
            };

            this.context.Reports.Add(report);
            this.context.SaveChanges();
        }

        public IEnumerable<ReportsViewModel> GetAllReports()
        {
            ReportsViewModel[] reports = this.context.Reports
                .Select(r => new ReportsViewModel
                {
                    Id = r.Id,
                    TaskTitle = r.Task.Title,
                    Level = r.Task.AffectedSectors.Count(),
                    Status = r.Status.ToString()
                })
                .ToArray();

            for (int i = 0; i < reports.Length; i++)
            {
                reports[i].Index = i + 1;
            }

            return reports;
        }

        public ReportsDetailsViewModel GetReport(int id)
        {
            ReportsDetailsViewModel report = this.context.Reports
                .Where(r => r.Id == id)
                .Select(r => new ReportsDetailsViewModel
                {
                    Id = r.Id,
                    TaskTitle = r.Task.Title,
                    Level = r.Task.AffectedSectors.Count(),
                    Status = r.Status.ToString(),
                    TaskDueDate = r.Task.DueDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReportedOn = r.ReportedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Reporter = r.Reporter.Username,
                    Participants = r.Task.Participants,
                    AffectedSectors = string.Join(", ", r.Task.AffectedSectors.Select(ts => ts.Sector)),
                    Description = r.Task.Description
                })
                .FirstOrDefault();

            return report;
        }
    }
}
