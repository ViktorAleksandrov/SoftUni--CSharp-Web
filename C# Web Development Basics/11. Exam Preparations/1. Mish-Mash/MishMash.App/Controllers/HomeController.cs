using MishMash.Services.Contracts;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;

namespace MishMash.App.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IChannelService channelService;

        public HomeController(IChannelService channelService)
        {
            this.channelService = channelService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!this.IsLoggedIn)
            {
                return this.View();
            }

            this.Model["Username"] = this.Identity.Username;
            this.Model["MyChannels"] = this.channelService.GetMyFollowedChannels(this.Identity.Username);
            this.Model["Suggested"] = this.channelService.GetSuggestedChannels(this.Identity.Username);
            this.Model["SeeOther"] = this.channelService.GetOtherChannels(this.Identity.Username);

            return this.View("LoggedInIndex");
        }
    }
}
