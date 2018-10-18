using SIS.HTTP.Enums;

namespace SIS.HTTP.Extensions
{
    public static class HttpResponseStatusExtensions
    {
        public static string GetResponseLine(this HttpResponseStatusCode statusCode)
        {
            string result;

            switch (statusCode)
            {
                case HttpResponseStatusCode.SeeOther:
                    result = $"{(int)statusCode} See Other";
                    break;
                case HttpResponseStatusCode.BadRequest:
                    result = $"{(int)statusCode} Bad Request";
                    break;
                case HttpResponseStatusCode.NotFound:
                    result = $"{(int)statusCode} Not Found";
                    break;
                case HttpResponseStatusCode.InternalServerError:
                    result = $"{(int)statusCode} Internal Server Error";
                    break;
                default:
                    result = $"{(int)statusCode} {statusCode}";
                    break;
            }

            return result;
        }
    }
}
