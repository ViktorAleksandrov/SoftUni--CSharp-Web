using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Requests;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.HTTP.Sessions;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;

namespace SIS.WebServer
{
    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IHttpHandler httpHandler;

        private readonly IHttpHandler resourceHandler;

        public ConnectionHandler(Socket client, IHttpHandler httpHandler, IHttpHandler resourceHandler)
        {
            this.client = client;
            this.httpHandler = httpHandler;
            this.resourceHandler = resourceHandler;
        }

        public async Task ProcessRequestAsync()
        {
            try
            {
                IHttpRequest httpRequest = await this.ReadRequest();

                if (httpRequest != null)
                {
                    string sessionId = this.SetRequestSession(httpRequest);

                    IHttpResponse httpResponse;

                    if (this.IsResourceRequest(httpRequest.Path))
                    {
                        httpResponse = this.resourceHandler.Handle(httpRequest);
                    }
                    else
                    {
                        httpResponse = this.httpHandler.Handle(httpRequest);
                    }

                    this.SetResponseSession(httpResponse, sessionId);

                    await this.PrepareResponse(httpResponse);
                }
            }
            catch (BadRequestException e)
            {
                await this.PrepareResponse(new TextResult(e.Message, HttpResponseStatusCode.BadRequest));
            }
            catch (Exception e)
            {
                await this.PrepareResponse(new TextResult(e.Message, HttpResponseStatusCode.InternalServerError));
            }

            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                string bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId = null;

            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                HttpCookie cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }

            return sessionId;
        }

        private bool IsResourceRequest(string path)
        {
            if (path.Contains('.'))
            {
                string fileExtension = path.Substring(path.LastIndexOf('.'));

                return GlobalConstants.ResourceExtensions.Contains(fileExtension);
            }

            return false;
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId)
        {
            if (sessionId != null)
            {
                httpResponse.AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey, sessionId));
            }
        }

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            await this.client.SendAsync(byteSegments, SocketFlags.None);
        }
    }
}
