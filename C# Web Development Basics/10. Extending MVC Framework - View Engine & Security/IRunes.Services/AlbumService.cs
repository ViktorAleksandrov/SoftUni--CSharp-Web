using System.Collections.Generic;
using System.Linq;
using IRunes.Data;
using IRunes.Models.Entities;
using IRunes.Models.ViewModels.Albums;
using IRunes.Services.Contracts;

namespace IRunes.Services
{
    public class AlbumService : IAlbumService
    {
        public void AddAlbum(AlbumViewModel model)
        {
            var album = new Album
            {
                Name = model.Name,
                Cover = model.Cover
            };

            using (var db = new IRunesContext())
            {
                db.Albums.Add(album);
                db.SaveChanges();
            }
        }

        public Album GetAlbum(string albumId)
        {
            using (var db = new IRunesContext())
            {
                Album album = db.Albums.Find(albumId);

                return album;
            }
        }

        public IEnumerable<Track> GetAlbumTracks(string albumId)
        {
            using (var db = new IRunesContext())
            {
                Track[] allTracks = db.Tracks
                    .Where(t => t.AlbumId == albumId)
                    .ToArray();

                return allTracks;
            }
        }

        public IEnumerable<Album> GetAllAlbums()
        {
            using (var db = new IRunesContext())
            {
                Album[] allAlbums = db.Albums.ToArray();

                return allAlbums;
            }
        }

        public decimal GetPrice(string albumId)
        {
            using (var db = new IRunesContext())
            {
                return db.Albums
                    .FirstOrDefault(a => a.Id == albumId)
                    .Tracks
                    .Sum(t => t.Price) * 0.87M;
            }
        }
    }
}
