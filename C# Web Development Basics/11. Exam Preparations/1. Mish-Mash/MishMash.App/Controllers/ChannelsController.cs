using System.Collections.Generic;
using MishMash.Services.Contracts;
using MishMash.ViewModels.Channels;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Methods;

namespace MishMash.App.Controllers
{
    public class ChannelsController : BaseController
    {
        private readonly IChannelService channelService;

        public ChannelsController(IChannelService channelService)
        {
            this.channelService = channelService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Follow(int id)
        {
            this.channelService.FollowChannel(id, this.Identity.Username);

            return this.RedirectToAction("/");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Unfollow(int id)
        {
            this.channelService.UnfollowChannel(id, this.Identity.Username);

            return this.RedirectToAction("/Channels/Followed");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            ChannelDetailsViewModel model = this.channelService.GetChannel(id);

            if (model == null)
            {
                return this.RedirectToAction("/");
            }

            this.Model["Name"] = model.Name;
            this.Model["Description"] = model.Description;
            this.Model["Type"] = model.Type;
            this.Model["Tags"] = model.Tags;
            this.Model["FollowersCount"] = model.FollowersCount;

            return this.View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Followed()
        {
            if (!this.IsLoggedIn)
            {
                return this.RedirectToAction("/");
            }

            IEnumerable<FollowedChannelViewModel> channels =
                this.channelService.GetFollowedChannels(this.Identity.Username);

            this.Model["Channels"] = channels;

            return this.View();
        }

        [HttpGet]
        [Authorize("Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize("Admin")]
        public IActionResult Create(ChannelDetailsViewModel model)
        {
            this.channelService.AddChannel(model);

            return this.RedirectToAction("/");
        }
    }
}
