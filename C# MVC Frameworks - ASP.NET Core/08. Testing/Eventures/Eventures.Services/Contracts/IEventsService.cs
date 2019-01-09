using System.Linq;
using Eventures.Models;

namespace Eventures.Services.Contracts
{
    public interface IEventsService
    {
        IQueryable<Event> GetAllEventsWithAvailableTickets();

        Event GetEventById(string eventId);

        IQueryable<Order> GetMyEvents(string username);

        void CreateEvent(Event @event);
    }
}
