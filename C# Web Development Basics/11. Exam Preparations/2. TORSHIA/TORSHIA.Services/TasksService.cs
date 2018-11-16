using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TORSHIA.Data;
using TORSHIA.Models;
using TORSHIA.Models.Enums;
using TORSHIA.Services.Base;
using TORSHIA.Services.Contracts;
using TORSHIA.ViewModels.Tasks;

namespace TORSHIA.Services
{
    public class TasksService : BaseService, ITasksService
    {
        private readonly IReportsService reportsService;

        public TasksService(TorshiaDbContext context, IReportsService reportsService)
            : base(context)
        {
            this.reportsService = reportsService;
        }

        public void CreateTask(TasksCreateViewModel model)
        {
            var task = new Task
            {
                Title = model.Title,
                DueDate = DateTime.Parse(model.DueDate),
                Description = model.Description,
                Participants = model.Participants
            };

            this.context.Tasks.Add(task);
            this.context.SaveChanges();

            if (model.AffectedSectors.Any())
            {
                foreach (string sector in model.AffectedSectors.Where(s => !string.IsNullOrWhiteSpace(s)))
                {
                    task.AffectedSectors.Add(new TaskSector { Sector = Enum.Parse<Sector>(sector) });
                }

                this.context.SaveChanges();
            }
        }

        public IEnumerable<IndexTasksViewModel> GetAllNonReportedTasks()
        {
            IndexTasksViewModel[] allNonReportedTasks = this.context.Tasks
                .Where(t => !t.IsReported)
                .Select(t => new IndexTasksViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Level = t.AffectedSectors.Count()
                })
                .ToArray();

            return allNonReportedTasks;
        }

        public TasksDetailsViewModel GetTask(int id)
        {
            TasksDetailsViewModel task = this.context.Tasks.Where(t => t.Id == id)
                .Select(t => new TasksDetailsViewModel
                {
                    Title = t.Title,
                    Level = t.AffectedSectors.Count(),
                    DueDate = t.DueDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Participants = t.Participants,
                    AffectedSectors = string.Join(", ", t.AffectedSectors.Select(ts => ts.Sector)),
                    Description = t.Description
                })
                .FirstOrDefault();

            return task;
        }

        public void ReportTask(int taskId, string username)
        {
            Task task = this.context.Tasks.Find(taskId);
            task.IsReported = true;

            this.reportsService.CreateReport(taskId, username);

            this.context.SaveChanges();
        }
    }
}
