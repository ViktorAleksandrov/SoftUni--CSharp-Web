using System.Collections.Generic;
using IRunes.Models.Entities;
using IRunes.Models.ViewModels.Albums;

namespace IRunes.Services.Contracts
{
    public interface IAlbumService
    {
        IEnumerable<Album> GetAllAlbums();

        void AddAlbum(AlbumViewModel model);

        Album GetAlbum(string albumId);

        decimal GetPrice(string albumId);

        IEnumerable<Track> GetAlbumTracks(string albumId);
    }
}
