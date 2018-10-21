using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;
using SIS.HTTP.Enums;
using SIS.HTTP.Extensions;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;

namespace SIS.Framework.Routers
{
    public class ControllerRouter : IHttpHandler
    {
        private const string ResourceDirectoryRelativePath = "../../../Resources/";

        public IHttpResponse Handle(IHttpRequest request)
        {
            string requestMethod = request.RequestMethod.ToString();

            string controllerName;
            string actionName;

            if (request.Path == "/")
            {
                controllerName = "Home";
                actionName = "Index";
            }
            else
            {
                string[] requestPathSplit = request.Path
                    .Split("/", StringSplitOptions.RemoveEmptyEntries);

                if (requestPathSplit.Length == 1)
                {
                    return new HttpResponse(HttpResponseStatusCode.NotFound);
                }

                controllerName = requestPathSplit[0].Capitalize();
                actionName = requestPathSplit[1].Capitalize();
            }

            Controller controller = this.GetController(controllerName, request);
            MethodInfo action = this.GetMethod(requestMethod, controller, actionName);

            if (controller == null || action == null)
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }

            return this.PrepareResponse(controller, action);
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            if (controllerName == null)
            {
                return null;
            }

            string controllerTypeName = string.Format(
                "{0}.{1}.{2}{3}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllersFolder,
                controllerName,
                MvcContext.Get.ControllersSuffix);

            var controllerType = Type.GetType(controllerTypeName);

            var controller = (Controller)Activator.CreateInstance(controllerType);

            if (controller != null)
            {
                controller.Request = request;
            }

            return controller;
        }

        private MethodInfo GetMethod(string requestMethod, Controller controller, string actionName)
        {
            MethodInfo method = null;

            IEnumerable<MethodInfo> actions = this.GetSuitableMethods(controller, actionName);

            foreach (MethodInfo methodInfo in actions)
            {
                IEnumerable<HttpMethodAttribute> attributes = methodInfo.GetCustomAttributes()
                    .Where(attr => attr is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                if (!attributes.Any() && requestMethod.ToUpper() == "GET")
                {
                    return methodInfo;
                }

                foreach (HttpMethodAttribute attribute in attributes)
                {
                    if (attribute.IsValid(requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return method;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller.GetType()
                .GetMethods()
                .Where(methodInfo => methodInfo.Name.ToLower() == actionName.ToLower());
        }

        private IHttpResponse PrepareResponse(Controller controller, MethodInfo action)
        {
            var actionResult = (IActionResult)action.Invoke(controller, null);

            string invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocationResult, HttpResponseStatusCode.Ok);
            }

            if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocationResult);
            }

            throw new InvalidOperationException("The view result is not supported.");
        }
    }
}
