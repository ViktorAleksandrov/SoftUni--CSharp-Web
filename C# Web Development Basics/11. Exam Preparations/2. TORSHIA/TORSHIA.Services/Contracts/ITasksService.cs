using System.Collections.Generic;
using TORSHIA.ViewModels.Tasks;

namespace TORSHIA.Services.Contracts
{
    public interface ITasksService
    {
        IEnumerable<IndexTasksViewModel> GetAllNonReportedTasks();

        TasksDetailsViewModel GetTask(int id);

        void CreateTask(TasksCreateViewModel model);

        void ReportTask(int taskId, string username);
    }
}
