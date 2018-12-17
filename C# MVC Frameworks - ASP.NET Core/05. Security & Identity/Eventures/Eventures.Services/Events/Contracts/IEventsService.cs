using System.Collections.Generic;
using Eventures.Models;

namespace Eventures.Services.Events.Contracts
{
    public interface IEventsService
    {
        IEnumerable<Event> GetAllEvents();

        IEnumerable<Order> GetMyEvents(string username);

        bool EventExists(string eventId);

        void CreateEvent(Event @event);
    }
}
