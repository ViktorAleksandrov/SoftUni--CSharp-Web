using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Routing;

namespace SIS.Demo
{
    class Launcher
    {
        static void Main(string[] args)
        {
            var serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index();

            var server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
