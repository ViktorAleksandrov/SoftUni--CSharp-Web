using System.Collections.Generic;
using MishMash.Models.Enums;

namespace MishMash.Models
{
    public class User
    {
        public User()
        {
            this.FollowedChannels = new HashSet<UserChannel>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public ICollection<UserChannel> FollowedChannels { get; set; }

        public Role Role { get; set; }
    }
}
