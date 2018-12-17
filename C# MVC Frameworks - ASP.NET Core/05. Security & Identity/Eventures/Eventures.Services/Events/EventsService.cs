using System.Collections.Generic;
using System.Linq;
using Eventures.Data;
using Eventures.Models;
using Eventures.Services.Events.Contracts;
using Microsoft.EntityFrameworkCore;

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

        public bool EventExists(string eventId)
        {
            return this.dbContext.Events.Any(e => e.Id == eventId);
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return this.dbContext.Events.ToArray();
        }

        public IEnumerable<Order> GetMyEvents(string username)
        {
            return this.dbContext.Orders
                .Where(o => o.Customer.UserName == username)
                .Include(o => o.Customer)
                .Include(o => o.Event)
                .ToArray();
        }
    }
}
