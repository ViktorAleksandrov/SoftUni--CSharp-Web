using SIS.Framework.Api;
using SIS.Framework.Services;
using TORSHIA.Services;
using TORSHIA.Services.Contracts;

namespace TORSHIA.App
{
    public class StartUp : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<IUsersService, UsersService>();
            dependencyContainer.RegisterDependency<ITasksService, TasksService>();
            dependencyContainer.RegisterDependency<IReportsService, ReportsService>();
        }
    }
}
