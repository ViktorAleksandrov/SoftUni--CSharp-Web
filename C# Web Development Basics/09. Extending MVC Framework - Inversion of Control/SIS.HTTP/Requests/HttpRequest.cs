using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Sessions.Contracts;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpSession Session { get; set; }

        private bool IsValidRequestLine(string[] requestLine)
        {
            return requestLine.Length == 3 && requestLine[2] == GlobalConstants.HttpOneProtocolFragment;
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            return !string.IsNullOrEmpty(queryString) && queryParameters.Any();
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            string capitalizedRequestMethod = StringExtensions.Capitalize(requestLine[0]);

            this.RequestMethod = Enum.Parse<HttpRequestMethod>(capitalizedRequestMethod);
        }

        private void ParseRequestUrl(string[] requestLine)
        {
            this.Url = requestLine[1];
        }

        private void ParseRequestPath()
        {
            this.Path = this.Url
                .Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();
        }

        private void ParseHeaders(string[] requestContent)
        {
            foreach (string headerLine in requestContent)
            {
                if (string.IsNullOrWhiteSpace(headerLine))
                {
                    break;
                }

                string[] headerLineParts = headerLine.Split(": ", StringSplitOptions.RemoveEmptyEntries);

                string headerKey = headerLineParts[0];
                string headerValue = headerLineParts[1];

                var header = new HttpHeader(headerKey, headerValue);

                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsHeader(GlobalConstants.HostHeaderKey))
            {
                throw new BadRequestException();
            }
        }

        private void ParseCookies()
        {
            if (!this.Headers.ContainsHeader("Cookie"))
            {
                return;
            }

            string cookiesString = this.Headers.GetHeader("Cookie").Value;

            if (string.IsNullOrEmpty(cookiesString))
            {
                return;
            }

            string[] splitCookies = cookiesString.Split("; ", StringSplitOptions.RemoveEmptyEntries);

            foreach (string splitCookie in splitCookies)
            {
                string[] cookieParts = splitCookie.Split('=', 2);

                string cookieKey = cookieParts[0];
                string cookieValue = cookieParts[1];

                var cookie = new HttpCookie(cookieKey, cookieValue, false);

                this.Cookies.Add(cookie);
            }
        }

        private void ParseQueryParameters()
        {
            string[] urlParts = this.Url.Split('?', StringSplitOptions.RemoveEmptyEntries);

            if (urlParts.Length > 1)
            {
                string queryString = urlParts[1];

                if (queryString.Contains('#'))
                {
                    queryString = queryString
                        .Split('#', StringSplitOptions.RemoveEmptyEntries)
                        .FirstOrDefault();
                }

                string[] queryParameters = queryString?.Split('&', StringSplitOptions.RemoveEmptyEntries);

                if (!this.IsValidRequestQueryString(queryString, queryParameters))
                {
                    throw new BadRequestException();
                }

                foreach (string parameter in queryParameters)
                {
                    string[] parameterParts = parameter.Split('=', StringSplitOptions.RemoveEmptyEntries);

                    string parameterKey = parameterParts[0];
                    string parameterValue = parameterParts[1];

                    this.QueryData[parameterKey] = parameterValue;
                }
            }
        }

        private void ParseFormDataParameters(string formData)
        {
            if (string.IsNullOrEmpty(formData))
            {
                return;
            }

            string[] bodyParameters = formData.Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (string parameter in bodyParameters)
            {
                string[] parameterParts = parameter.Split('=', 2, StringSplitOptions.None);

                string parameterKey = parameterParts[0];
                string parameterValue = HttpUtility.UrlDecode(parameterParts[1]);

                this.FormData[parameterKey] = parameterValue;
            }
        }

        private void ParseRequestParameters(string formData)
        {
            this.ParseQueryParameters();
            this.ParseFormDataParameters(formData);
        }

        private void ParseRequest(string requestString)
        {
            string[] splitRequestContent = requestString.Split(Environment.NewLine);

            string[] requestLine = splitRequestContent[0]
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);
            this.ParseRequestUrl(requestLine);
            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }
    }
}
