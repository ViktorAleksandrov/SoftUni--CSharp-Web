using System.Linq;
using System.Text;
using System.Web;
using IRunes.Models;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunes.App.Controllers
{
    public class TracksController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            string albumId = request.QueryData["albumId"].ToString();

            this.ViewBag["albumId"] = albumId;

            return this.View();
        }

        public IHttpResponse PostCreate(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            string albumId = request.QueryData["albumId"].ToString();

            string name = request.FormData["name"].ToString();
            string link = request.FormData["link"].ToString();
            bool isNumber = decimal.TryParse(request.FormData["price"].ToString(), out decimal price);

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(link) || !isNumber || price <= 0)
            {
                return this.View("Create");
            }

            var track = new Track
            {
                Name = name,
                Link = link,
                Price = price,
                AlbumId = albumId
            };

            this.Db.Tracks.Add(track);
            this.Db.SaveChanges();

            return new RedirectResult($"/Albums/Details?id={albumId}");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            string albumId = request.QueryData["albumId"].ToString();
            string trackId = request.QueryData["trackId"].ToString();

            Track track = this.Db.Tracks.FirstOrDefault(t => t.Id == trackId);

            string trackLink = HttpUtility.UrlDecode(track.Link).Replace("watch?v=", "embed/");

            var trackDetails = new StringBuilder();

            string priceWithComma = track.Price.ToString("F2").Replace('.', ',');

            trackDetails.Append($@"<iframe src=""{trackLink}""></iframe>");
            trackDetails.Append($@"<h3>Name: {track.Name}</h3></ br>");
            trackDetails.Append($@"<h3>Price: &#36;{priceWithComma}</h3></ br>");

            this.ViewBag["trackDetails"] = trackDetails.ToString();
            this.ViewBag["albumId"] = albumId;

            return this.View();
        }
    }
}
