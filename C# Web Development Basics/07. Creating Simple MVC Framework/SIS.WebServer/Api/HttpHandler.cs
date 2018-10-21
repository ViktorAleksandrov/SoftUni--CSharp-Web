using System;
using System.IO;
using System.Linq;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;

namespace SIS.WebServer.Api
{
    public class HttpHandler : IHttpHandler
    {
        private const string ResourceDirectoryRelativePath = "../../../Resources/";

        private readonly ServerRoutingTable serverRoutingTable;

        public HttpHandler(ServerRoutingTable serverRoutingTable)
        {
            this.serverRoutingTable = serverRoutingTable;
        }

        public IHttpResponse Handle(IHttpRequest request)
        {
            if (!this.serverRoutingTable.Routes.ContainsKey(request.RequestMethod)
                || !this.serverRoutingTable.Routes[request.RequestMethod].ContainsKey(request.Path))
            {
                return this.ReturnIfResource(request.Path);
            }

            return this.serverRoutingTable.Routes[request.RequestMethod][request.Path].Invoke(request);
        }

        private IHttpResponse ReturnIfResource(string path)
        {
            if (path.Contains('.'))
            {
                string fileExtension = path.Substring(path.LastIndexOf('.'));

                if (GlobalConstants.ResourceExtensions.Contains(fileExtension))
                {
                    fileExtension = fileExtension.Substring(1);

                    string resourcePath = path.Substring(path.LastIndexOf('/'));

                    string pathToSearch = $"{ResourceDirectoryRelativePath}{fileExtension}{resourcePath}";

                    if (File.Exists(pathToSearch))
                    {
                        byte[] fileContent = File.ReadAllBytes(pathToSearch);

                        return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);
                    }
                }
            }

            return new HttpResponse(HttpResponseStatusCode.NotFound);
        }
    }
}
