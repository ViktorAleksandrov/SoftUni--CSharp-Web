using System;
using System.Linq;
using Eventures.Web.ViewModels.Events;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Eventures.Web.Filters
{
    public class LogAdminEventCreateActionFilter : ActionFilterAttribute
    {
        private readonly ILogger logger;
        private CreateEventViewModel model;

        public LogAdminEventCreateActionFilter(ILogger<LogAdminEventCreateActionFilter> logger)
        {
            this.logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.model = context.ActionArguments.Values.OfType<CreateEventViewModel>().Single();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (this.model != null)
            {
                string adminUsername = context.HttpContext.User.Identity.Name;

                string logMessage =
                    $"[{DateTime.Now}] Administrator {adminUsername} create event {this.model.Name}" +
                    $" ({this.model.Start} / {this.model.End})";

                this.logger.LogInformation(logMessage);
            }
        }
    }
}
