using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;
using SIS.Framework.Services.Contracts;
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
        private const string UnsupportedActionMessage = "The view result is not supported.";

        private readonly IDependencyContainer dependencyContainer;

        public ControllerRouter(IDependencyContainer dependencyContainer)
        {
            this.dependencyContainer = dependencyContainer;
        }

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

            object[] actionParameters = this.MapActionParameters(action, request, controller);

            IActionResult actionResult = this.InvokeAction(controller, action, actionParameters);

            return this.PrepareResponse(actionResult);
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

            var controller = (Controller)this.dependencyContainer.CreateInstance(controllerType);

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

        private object[] MapActionParameters(MethodInfo action, IHttpRequest httpRequest, Controller controller)
        {
            ParameterInfo[] actionParametersInfo = action.GetParameters();

            object[] mappedActionParameters = new object[actionParametersInfo.Length];

            for (int index = 0; index < actionParametersInfo.Length; index++)
            {
                ParameterInfo currentParameterInfo = actionParametersInfo[index];

                if (currentParameterInfo.ParameterType.IsPrimitive
                    || currentParameterInfo.ParameterType == typeof(string))
                {
                    mappedActionParameters[index] = this.ProcessPrimitiveParameter(currentParameterInfo, httpRequest);
                }
                else
                {
                    object bindingModel = this.ProcessBindingModelParameters(currentParameterInfo, httpRequest);

                    controller.ModelState.IsValid = this.IsValidModel(bindingModel);

                    mappedActionParameters[index] = bindingModel;
                }
            }

            return mappedActionParameters;
        }

        private object ProcessPrimitiveParameter(ParameterInfo param, IHttpRequest httpRequest)
        {
            object value = this.GetParameterFromRequestData(httpRequest, param.Name);

            return Convert.ChangeType(value, param.ParameterType);
        }

        private object GetParameterFromRequestData(IHttpRequest httpRequest, string paramName)
        {
            if (httpRequest.QueryData.ContainsKey(paramName))
            {
                return httpRequest.QueryData[paramName];
            }

            if (httpRequest.FormData.ContainsKey(paramName))
            {
                return httpRequest.FormData[paramName];
            }

            return null;
        }

        private object ProcessBindingModelParameters(ParameterInfo param, IHttpRequest httpRequest)
        {
            Type bindingModelType = param.ParameterType;

            object bindingModelInstance = Activator.CreateInstance(bindingModelType);

            PropertyInfo[] bindingModelProperties = bindingModelType.GetProperties();

            foreach (PropertyInfo property in bindingModelProperties)
            {
                try
                {
                    object value = this.GetParameterFromRequestData(httpRequest, property.Name);

                    property.SetValue(bindingModelInstance, Convert.ChangeType(value, property.PropertyType));
                }
                catch
                {
                    Console.WriteLine($"The {property.Name} field could not be mapped.");
                }
            }

            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }

        private bool IsValidModel(object bindingModel)
        {
            PropertyInfo[] properties = bindingModel.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                IEnumerable<ValidationAttribute> propertyValidationAttributes = property
                    .GetCustomAttributes()
                    .Where(a => a is ValidationAttribute)
                    .Cast<ValidationAttribute>();

                foreach (ValidationAttribute validationAttribute in propertyValidationAttributes)
                {
                    object propertyValue = property.GetValue(bindingModel);

                    if (!validationAttribute.IsValid(propertyValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private IActionResult InvokeAction(Controller controller, MethodInfo action, object[] actionParameters)
        {
            return (IActionResult)action.Invoke(controller, actionParameters);
        }

        private IHttpResponse PrepareResponse(IActionResult actionResult)
        {
            string invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocationResult, HttpResponseStatusCode.Ok);
            }

            if (actionResult is IRedirectable)
            {
                return new RedirectResult(invocationResult);
            }

            throw new InvalidOperationException(UnsupportedActionMessage);
        }
    }
}
