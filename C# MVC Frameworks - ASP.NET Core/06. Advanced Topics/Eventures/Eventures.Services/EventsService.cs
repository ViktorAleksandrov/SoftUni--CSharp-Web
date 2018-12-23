using System.Collections.Generic;
using System.Linq;
using Eventures.Data;
using Eventures.Models;
using Eventures.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Eventures.Services
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
            return this.dbContext.Events.Where(e => e.TotalTickets > 0).ToArray();
        }

        public Event GetEventById(string eventId)
        {
            return this.dbContext.Events.SingleOrDefault(e => e.Id == eventId);
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
