using SIS.Demo.Controllers;
using SIS.Framework.Api;
using SIS.Framework.Services.Contracts;

namespace SIS.Demo
{
    public class StartUp : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<HomeController, HomeController>();
        }
    }
}
