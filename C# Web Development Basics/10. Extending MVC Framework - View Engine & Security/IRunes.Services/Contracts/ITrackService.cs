using IRunes.Models.Entities;
using IRunes.Models.ViewModels.Tracks;

namespace IRunes.Services.Contracts
{
    public interface ITrackService
    {
        void AddTrack(TrackViewModel model);

        Track GetTrack(string trackId);
    }
}
