using System.Collections.Generic;
using Eventures.Models;

namespace Eventures.Services.Events.Contracts
{
    public interface IEventsService
    {
        IEnumerable<Event> GetAllEvents();

        void CreateEvent(Event @event);
    }
}
