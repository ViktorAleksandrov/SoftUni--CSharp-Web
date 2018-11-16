using System.Collections.Generic;
using MishMash.ViewModels.Channels;

namespace MishMash.Services.Contracts
{
    public interface IChannelService
    {
        void AddChannel(ChannelDetailsViewModel model);

        ChannelDetailsViewModel GetChannel(int id);

        IEnumerable<FollowedChannelViewModel> GetFollowedChannels(string username);

        IEnumerable<MyChannelViewModel> GetMyFollowedChannels(string username);

        IEnumerable<ChannelViewModel> GetSuggestedChannels(string username);

        IEnumerable<ChannelViewModel> GetOtherChannels(string username);

        void FollowChannel(int channelId, string username);

        void UnfollowChannel(int channelId, string username);
    }
}
