using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace SIS.WebServer.Api.Contracts
{
    public interface IHttpHandler
    {
        IHttpResponse Handle(IHttpRequest request);
    }
}
