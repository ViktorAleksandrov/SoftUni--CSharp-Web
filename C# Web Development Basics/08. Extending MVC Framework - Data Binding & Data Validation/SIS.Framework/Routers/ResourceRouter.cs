using System.IO;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;

namespace SIS.Framework.Routers
{
    public class ResourceRouter : IHttpHandler
    {
        private const string ResourceDirectoryRelativePath = "../../../Resources/";

        public IHttpResponse Handle(IHttpRequest request)
        {
            string path = request.Path;

            string fileExtension = path.Substring(path.LastIndexOf('.') + 1);
            string resourcePath = path.Substring(path.LastIndexOf('/'));

            string pathToSearch = $"{ResourceDirectoryRelativePath}{fileExtension}{resourcePath}";

            if (!File.Exists(pathToSearch))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            byte[] fileContent = File.ReadAllBytes(pathToSearch);

            return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);
        }
    }
}
