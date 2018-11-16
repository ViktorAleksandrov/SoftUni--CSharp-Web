using MishMash.Services;
using MishMash.Services.Contracts;
using SIS.Framework.Api;
using SIS.Framework.Services.Contracts;

namespace MishMash.App
{
    public class StartUp : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<IUserService, UserService>();
            dependencyContainer.RegisterDependency<IChannelService, ChannelService>();
        }
    }
}
