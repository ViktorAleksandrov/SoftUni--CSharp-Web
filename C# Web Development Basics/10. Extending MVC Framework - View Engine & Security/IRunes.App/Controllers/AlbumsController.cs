using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using IRunes.Models.Entities;
using IRunes.Models.ViewModels.Albums;
using IRunes.Services.Contracts;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;

namespace IRunes.App.Controllers
{
    public class AlbumsController : Controller
    {
        private const string Display = "display";
        private const string None = "none";
        private const string ErrorMessage = "errorMessage";
        private const string Inline = "inline";

        private readonly IAlbumService albumService;

        public AlbumsController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            this.SetViewByUserAuthentication();

            IEnumerable<Album> albums = this.albumService.GetAllAlbums();

            var albumsList = new StringBuilder();

            if (albums.Any())
            {
                foreach (Album album in albums)
                {
                    string albumHtml =
                        $@"<p><strong><a href =""/Albums/Details?id={album.Id}"">{album.Name}</a></strong></p>";

                    albumsList.Append(albumHtml);
                }
            }
            else
            {
                albumsList.Append("<h3>There are currently no albums.</h3>");
            }

            this.Model["albumsList"] = albumsList.ToString();

            return this.View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            this.SetViewByUserAuthentication();

            this.Model[Display] = None;

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AlbumViewModel model)
        {
            this.SetViewByUserAuthentication();

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Cover))
            {
                this.Model[Display] = Inline;
                this.Model[ErrorMessage] = "All fields must be filled!";

                return this.View();
            }

            this.albumService.AddAlbum(model);

            return this.RedirectToAction("/Albums/All");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(AlbumViewModel model)
        {
            this.SetViewByUserAuthentication();

            string albumId = model.Id;

            IEnumerable<Track> albumTracks = this.albumService.GetAlbumTracks(albumId);

            var tracksList = new StringBuilder();

            if (albumTracks.Any())
            {
                tracksList.Append("<ul>");

                int songCounter = 1;

                foreach (Track track in albumTracks)
                {
                    string trackHtml = $@"<li><b>{songCounter++}</b>. <em><a href=""/Tracks/Details?albumid={albumId}&trackid={track.Id}"">{track.Name}</a></em></ li>";

                    tracksList.Append(trackHtml);
                }

                tracksList.Append("</ul>");
            }
            else
            {
                tracksList.Append("<h3>There are no tracks in this album.</h3>");
            }

            Album album = this.albumService.GetAlbum(albumId);

            this.Model["cover"] = HttpUtility.UrlDecode(album.Cover);
            this.Model["name"] = album.Name;
            this.Model["price"] = this.albumService.GetPrice(albumId).ToString("F2").Replace('.', ',');
            this.Model["albumId"] = albumId;
            this.Model["tracksList"] = tracksList.ToString();

            return this.View();
        }
    }
}
