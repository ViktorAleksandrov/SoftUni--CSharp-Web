using SIS.Framework;
using SIS.Framework.Routers;
using SIS.WebServer;

namespace SIS.Demo
{
    class Launcher
    {
        static void Main(string[] args)
        {
            var server = new Server(80, new ControllerRouter());

            MvcEngine.Run(server);
        }
    }
}
