using System;
using System.Collections.Generic;
using System.Linq;
using MishMash.Data;
using MishMash.Models;
using MishMash.Models.Enums;
using MishMash.Services.Contracts;
using MishMash.ViewModels.Channels;

namespace MishMash.Services
{
    public class ChannelService : IChannelService
    {
        public void AddChannel(ChannelDetailsViewModel model)
        {
            using (var db = new MishMashDbContext())
            {
                var channel = new Channel
                {
                    Name = model.Name,
                    Description = model.Description,
                    Type = Enum.Parse<ChannelType>(model.Type)
                };

                if (!string.IsNullOrWhiteSpace(model.Tags))
                {
                    string[] tags = model.Tags.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string tag in tags)
                    {
                        Tag dbTag = db.Tags.FirstOrDefault(t => t.Name == tag);

                        if (dbTag == null)
                        {
                            dbTag = new Tag { Name = tag };

                            db.Tags.Add(dbTag);
                            db.SaveChanges();
                        }

                        channel.Tags.Add(new TagChannel { TagId = dbTag.Id });
                    }
                }

                db.Channels.Add(channel);
                db.SaveChanges();
            }
        }

        public ChannelDetailsViewModel GetChannel(int id)
        {
            using (var db = new MishMashDbContext())
            {
                ChannelDetailsViewModel model = db.Channels.Where(c => c.Id == id)
                    .Select(c => new ChannelDetailsViewModel
                    {
                        Name = c.Name,
                        Description = c.Description,
                        Tags = string.Join(", ", c.Tags.Select(ct => ct.Tag.Name)),
                        Type = c.Type.ToString(),
                        FollowersCount = c.Followers.Count()
                    })
                    .FirstOrDefault();

                return model;
            }
        }

        public IEnumerable<FollowedChannelViewModel> GetFollowedChannels(string username)
        {
            using (var db = new MishMashDbContext())
            {
                FollowedChannelViewModel[] channels = db.Channels
                    .Where(c => c.Followers.Any(uc => uc.User.Username == username))
                    .Select(c => new FollowedChannelViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Type = c.Type.ToString(),
                        FollowersCount = c.Followers.Count()
                    })
                    .ToArray();

                for (int i = 0; i < channels.Length; i++)
                {
                    channels[i].Index = i + 1;
                }

                return channels;
            }
        }

        public IEnumerable<MyChannelViewModel> GetMyFollowedChannels(string username)
        {
            using (var db = new MishMashDbContext())
            {
                MyChannelViewModel[] channels = db.Channels
                    .Where(c => c.Followers.Any(uc => uc.User.Username == username))
                    .Select(c => new MyChannelViewModel
                    {
                        Name = c.Name,
                        Type = c.Type.ToString(),
                        FollowersCount = c.Followers.Count()
                    })
                    .ToArray();

                return channels;
            }
        }

        public IEnumerable<ChannelViewModel> GetSuggestedChannels(string username)
        {
            using (var db = new MishMashDbContext())
            {
                int[] followedChannelsTags = db.Channels
                    .Where(c => c.Followers.Any(uc => uc.User.Username == username))
                    .SelectMany(c => c.Tags.Select(tc => tc.Tag.Id))
                    .ToArray();

                ChannelViewModel[] channels = db.Channels
                    .Where(c =>
                        c.Followers.All(uc => uc.User.Username != username) &&
                        c.Tags.Any(tc => followedChannelsTags.Contains(tc.TagId)))
                    .Select(c => new ChannelViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Type = c.Type.ToString(),
                        FollowersCount = c.Followers.Count()
                    })
                    .ToArray();

                return channels;
            }
        }

        public IEnumerable<ChannelViewModel> GetOtherChannels(string username)
        {
            using (var db = new MishMashDbContext())
            {
                int[] followedChannelsIds = this.GetFollowedChannels(username)
                    .Select(m => m.Id)
                    .ToArray();

                int[] suggestedChannelsIds = this.GetSuggestedChannels(username)
                    .Select(m => m.Id)
                    .ToArray();

                int[] ids = followedChannelsIds.Concat(suggestedChannelsIds)
                    .Distinct()
                    .ToArray();

                ChannelViewModel[] channels = db.Channels
                    .Where(c => !ids.Contains(c.Id))
                    .Select(c => new ChannelViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Type = c.Type.ToString(),
                        FollowersCount = c.Followers.Count()
                    })
                    .ToArray();

                return channels;
            }
        }

        public void FollowChannel(int channelId, string username)
        {
            using (var db = new MishMashDbContext())
            {
                int userId = db.Users.FirstOrDefault(u => u.Username == username).Id;

                if (!db.UserChannels.Any(uc => uc.UserId == userId && uc.ChannelId == channelId))
                {
                    db.UserChannels.Add(new UserChannel
                    {
                        UserId = userId,
                        ChannelId = channelId
                    });

                    db.SaveChanges();
                }
            }
        }

        public void UnfollowChannel(int channelId, string username)
        {
            using (var db = new MishMashDbContext())
            {
                int userId = db.Users.FirstOrDefault(u => u.Username == username).Id;

                UserChannel userChannel = db.UserChannels
                    .FirstOrDefault(uc => uc.UserId == userId && uc.ChannelId == channelId);

                if (userChannel != null)
                {
                    db.UserChannels.Remove(userChannel);

                    db.SaveChanges();
                }
            }
        }
    }
}
