using SIS.Framework;
using SIS.Framework.Routers;
using SIS.Framework.Services;
using SIS.WebServer;

namespace SIS.Demo
{
    class Launcher
    {
        static void Main(string[] args)
        {
            var server = new Server(80, new ControllerRouter(new DependencyContainer()), new ResourceRouter());

            MvcEngine.Run(server);
        }
    }
}
