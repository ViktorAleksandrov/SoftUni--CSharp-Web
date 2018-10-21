using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SIS.WebServer.Api.Contracts;

namespace SIS.WebServer
{
    public class Server
    {
        private const string LocalhostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener listener;

        private readonly IHttpHandler httpHandler;

        private bool isRunning;

        public Server(int port, IHttpHandler httpHandler)
        {
            this.port = port;
            this.listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);

            this.httpHandler = httpHandler;
        }

        public void Run()
        {
            this.listener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalhostIpAddress}:{this.port}");

            var task = Task.Run(this.ListenLoop);
            task.Wait();
        }

        public async Task ListenLoop()
        {
            while (this.isRunning)
            {
                Console.WriteLine("Waiting for client...");

                Socket client = await this.listener.AcceptSocketAsync();

                var connectionHandler = new ConnectionHandler(client, this.httpHandler);

                Task responseTask = connectionHandler.ProcessRequestAsync();
                responseTask.Wait();
            }
        }
    }
}
