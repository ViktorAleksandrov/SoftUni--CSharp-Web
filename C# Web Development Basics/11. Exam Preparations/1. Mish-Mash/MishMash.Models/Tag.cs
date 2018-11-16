using System.Collections.Generic;

namespace MishMash.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Channels = new HashSet<TagChannel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<TagChannel> Channels { get; set; }
    }
}
