using CHUSHKA.Services;
using CHUSHKA.Services.Contracts;
using SIS.Framework.Api;
using SIS.Framework.Services;

namespace CHUSHKA.App
{
    public class StartUp : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<IUsersService, UsersService>();
            dependencyContainer.RegisterDependency<IProductsService, ProductsService>();
            dependencyContainer.RegisterDependency<IOrdersService, OrdersService>();
        }
    }
}
