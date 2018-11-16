using System.Collections.Generic;
using TORSHIA.ViewModels.Reports;

namespace TORSHIA.Services.Contracts
{
    public interface IReportsService
    {
        IEnumerable<ReportsViewModel> GetAllReports();

        ReportsDetailsViewModel GetReport(int id);

        void CreateReport(int taskId, string username);
    }
}
