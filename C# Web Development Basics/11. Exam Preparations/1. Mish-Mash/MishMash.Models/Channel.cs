using System.Collections.Generic;
using MishMash.Models.Enums;

namespace MishMash.Models
{
    public class Channel
    {
        public Channel()
        {
            this.Tags = new HashSet<TagChannel>();
            this.Followers = new HashSet<UserChannel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ChannelType Type { get; set; }

        public ICollection<TagChannel> Tags { get; set; }

        public ICollection<UserChannel> Followers { get; set; }
    }
}
