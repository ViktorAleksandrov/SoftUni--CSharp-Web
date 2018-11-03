using IRunes.Data;
using IRunes.Models.Entities;
using IRunes.Models.ViewModels.Tracks;
using IRunes.Services.Contracts;

namespace IRunes.Services
{
    public class TrackService : ITrackService
    {
        public void AddTrack(TrackViewModel model)
        {
            var track = new Track
            {
                Name = model.Name,
                Link = model.Link,
                Price = decimal.Parse(model.Price),
                AlbumId = model.AlbumId
            };

            using (var db = new IRunesContext())
            {
                db.Tracks.Add(track);
                db.SaveChanges();
            }
        }

        public Track GetTrack(string trackId)
        {
            using (var db = new IRunesContext())
            {
                Track track = db.Tracks.Find(trackId);

                return track;
            }
        }
    }
}
