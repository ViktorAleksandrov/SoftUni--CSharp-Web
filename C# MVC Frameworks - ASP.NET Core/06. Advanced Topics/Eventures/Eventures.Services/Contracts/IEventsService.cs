using System.Collections.Generic;
using Eventures.Models;

namespace Eventures.Services.Contracts
{
    public interface IEventsService
    {
        IEnumerable<Event> GetAllEvents();

        Event GetEventById(string eventId);

        IEnumerable<Order> GetMyEvents(string username);

        void CreateEvent(Event @event);
    }
}
