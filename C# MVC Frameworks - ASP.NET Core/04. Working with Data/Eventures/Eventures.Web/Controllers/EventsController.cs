using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Eventures.Models;
using Eventures.Services.Events.Contracts;
using Eventures.Web.Filters;
using Eventures.Web.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Eventures.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventsService eventsService;
        private readonly ILogger logger;

        public EventsController(IEventsService eventsService, ILogger<EventsController> logger)
        {
            this.eventsService = eventsService;
            this.logger = logger;
        }

        [Authorize]
        public IActionResult All()
        {
            IEnumerable<AllEventsViewModel> events = this.eventsService.GetAllEvents()
                .Select(e => new AllEventsViewModel
                {
                    Name = e.Name,
                    Start = e.Start.ToString("dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture),
                    End = e.End.ToString("dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture),
                    Place = e.Place
                });

            return this.View(events);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [TypeFilter(typeof(LogAdminCreateEventActionFilter))]
        public IActionResult Create(CreateEventBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                var @event = new Event
                {
                    Name = model.Name,
                    Start = model.Start,
                    End = model.End,
                    Place = model.Place,
                    TotalTickets = model.TotalTickets,
                    TicketPrice = model.PricePerTicket
                };

                this.eventsService.CreateEvent(@event);

                this.logger.LogInformation($"Event created: {@event.Name}", @event);

                return this.RedirectToAction(nameof(All));
            }

            return this.View();
        }
    }
}