﻿using System.IO;
using System.Linq;
using SIS.Framework.Routers.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;

namespace SIS.Framework.Routers
{
    public class ResourceRouter : IResourceRouter
    {
        private const string RootDirectoryRelativePath = "../../../";

        private const string ResourceFolderPath = "Resources/";

        private static readonly string[] AllowedResourceExtensions = { ".js", ".css", ".ico", ".jpg", ".jpeg", ".png", ".gif", ".html" };

        private string FormatResourcePath(string httpRequestPath)
        {
            int indexOfStartOfExtension = httpRequestPath.LastIndexOf('.');
            int indexOfStartOfNameOfResource = httpRequestPath.LastIndexOf('/');

            string requestPathExtension = httpRequestPath
                .Substring(indexOfStartOfExtension);

            string resourceName = httpRequestPath
                .Substring(
                    indexOfStartOfNameOfResource);

            return RootDirectoryRelativePath
                               + ResourceFolderPath
                               + requestPathExtension.Substring(1)
                               + resourceName;
        }

        private bool IsAllowedExtension(string httpRequestPath)
        {
            string requestPathExtension = httpRequestPath
                .Substring(httpRequestPath.LastIndexOf('.'));

            return AllowedResourceExtensions.Contains(requestPathExtension);
        }

        public bool IsResourceRequest(string httpRequestPath) => httpRequestPath.Contains('.');

        public IHttpResponse Handle(IHttpRequest httpRequest)
        {
            if (this.IsAllowedExtension(httpRequest.Path))
            {
                string httpRequestPath = httpRequest.Path;

                string resourcePath = this.FormatResourcePath(httpRequestPath);

                if (!File.Exists(resourcePath))
                {
                    return null;
                }

                byte[] fileContent = File.ReadAllBytes(resourcePath);

                return new InlineResouceResult(fileContent, HttpResponseStatusCode.Ok);
            }

            return null;
        }
    }
}
