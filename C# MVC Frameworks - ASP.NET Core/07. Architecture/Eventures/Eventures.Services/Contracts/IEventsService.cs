using System.Linq;
using Eventures.Models;

namespace Eventures.Services.Contracts
{
    public interface IEventsService
    {
        IQueryable<Event> GetAllEvents();

        Event GetEventById(string eventId);

        IQueryable<Order> GetMyEvents(string username);

        void CreateEvent(Event @event);
    }
}
