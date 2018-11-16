using SIS.Framework.Api.Contracts;
using SIS.Framework.Routers;
using SIS.Framework.Services;
using SIS.Framework.Services.Contracts;
using SIS.WebServer;
using SIS.WebServer.Api.Contracts;

namespace SIS.Framework
{
    public static class WebHost
    {
        private const int HostingPort = 80;

        public static void Start(IMvcApplication application)
        {
            IDependencyContainer container = new DependencyContainer();

            application.ConfigureServices(container);

            IHttpHandler controllerRouter = new ControllerRouter(container);

            application.Configure();

            var server = new Server(HostingPort, controllerRouter);

            server.Run();
        }
    }
}
