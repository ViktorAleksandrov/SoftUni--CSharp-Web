using System.Collections.Generic;
using System.Linq;
using Eventures.Data;
using Eventures.Models;
using Eventures.Services.Events.Contracts;

namespace Eventures.Services.Events
{
    public class EventsService : IEventsService
    {
        private readonly EventuresDbContext dbContext;

        public EventsService(EventuresDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateEvent(Event @event)
        {
            this.dbContext.Events.Add(@event);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return this.dbContext.Events.ToArray();
        }
    }
}
