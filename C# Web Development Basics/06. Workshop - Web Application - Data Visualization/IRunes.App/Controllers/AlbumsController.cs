using System.Linq;
using System.Text;
using System.Web;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunes.App.Controllers
{
    public class AlbumsController : BaseController
    {
        public IHttpResponse All(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            DbSet<Album> albums = this.Db.Albums;

            string listOfAlbums = string.Empty;

            if (albums.Any())
            {
                foreach (Album album in albums)
                {
                    string albumHtml =
                        $@"<p><strong><a href =""/Albums/Details?id={album.Id}"">{album.Name}</a></strong></p>";

                    listOfAlbums += albumHtml;
                }

                this.ViewBag["albumsList"] = listOfAlbums;
            }
            else
            {
                this.ViewBag["albumsList"] = "There are currently no albums.";
            }

            return this.View();
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            return this.View();
        }

        public IHttpResponse PostCreate(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            string name = request.FormData["name"].ToString();
            string cover = request.FormData["cover"].ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(cover))
            {
                return this.View("Create");
            }

            var album = new Album
            {
                Name = name,
                Cover = cover
            };

            this.Db.Albums.Add(album);
            this.Db.SaveChanges();

            return new RedirectResult("/Albums/All");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            string albumId = request.QueryData["id"].ToString();

            Album album = this.Db.Albums.FirstOrDefault(a => a.Id == albumId);

            string albumCover = HttpUtility.UrlDecode(album.Cover);

            var albumDetails = new StringBuilder();

            string priceWithComma = album.Price.ToString("F2").Replace('.', ',');

            albumDetails.Append($@"<img src=""{albumCover}"" height=""300"" width=""450""/></ br>");
            albumDetails.Append($@"<h3>Album Name: {album.Name}</h3></ br>");
            albumDetails.Append($@"<h3>Album Price: &#36;{priceWithComma}</h3></ br>");

            var listOfTracks = new StringBuilder();

            if (album.Tracks.Any())
            {
                listOfTracks.Append("<ul>");

                int songCounter = 1;

                foreach (Track track in album.Tracks)
                {
                    string trackHtml = $@"<li><b>{songCounter++}</b>. <em><a href=""/Tracks/Details?albumId={album.Id}&trackId={track.Id}"">{track.Name}</a></em></ li>";

                    listOfTracks.Append(trackHtml);
                }

                listOfTracks.Append("</ul>");
            }

            this.ViewBag["albumDetails"] = albumDetails.ToString();
            this.ViewBag["albumId"] = albumId;
            this.ViewBag["tracksList"] = listOfTracks.ToString();

            return this.View();
        }
    }
}
