using System.Web;
using IRunes.Models.Entities;
using IRunes.Models.ViewModels.Tracks;
using IRunes.Services.Contracts;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;

namespace IRunes.App.Controllers
{
    public class TracksController : Controller
    {
        private const string Display = "display";
        private const string None = "none";
        private const string ErrorMessage = "errorMessage";
        private const string Inline = "inline";

        private readonly ITrackService trackService;

        public TracksController(ITrackService trackService)
        {
            this.trackService = trackService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(AlbumIdViewModel model)
        {
            this.SetViewByUserAuthentication();

            this.Model[Display] = None;

            this.Model["albumId"] = model.AlbumId;

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(TrackViewModel model)
        {
            this.SetViewByUserAuthentication();

            this.Model["albumId"] = model.AlbumId;

            if (string.IsNullOrWhiteSpace(model.Name)
                || string.IsNullOrWhiteSpace(model.Link)
                || string.IsNullOrWhiteSpace(model.Price))
            {
                this.Model[Display] = Inline;
                this.Model[ErrorMessage] = "All fields must be filled!";

                return this.View();
            }

            bool isNumber = decimal.TryParse(model.Price, out decimal price);

            if (!isNumber)
            {
                this.Model[Display] = Inline;
                this.Model[ErrorMessage] = "The price must be a number!";

                return this.View();
            }

            if (price <= 0)
            {
                this.Model[Display] = Inline;
                this.Model[ErrorMessage] = "The price must be more than zero!";

                return this.View();
            }

            this.trackService.AddTrack(model);

            return this.RedirectToAction($"/Albums/Details?id={model.AlbumId}");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(TrackViewModel model)
        {
            this.SetViewByUserAuthentication();

            Track track = this.trackService.GetTrack(model.TrackId);

            this.Model["name"] = track.Name;
            this.Model["price"] = track.Price.ToString("F2").Replace('.', ',');
            this.Model["link"] = HttpUtility.UrlDecode(track.Link).Replace("watch?v=", "embed/");
            this.Model["albumId"] = model.AlbumId;

            return this.View();
        }
    }
}
