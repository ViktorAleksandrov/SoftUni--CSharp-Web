using System;
using System.Globalization;
using System.Linq;
using Eventures.Models;
using Eventures.Services.Events.Contracts;
using Eventures.Services.Orders.Contracts;
using Eventures.Web.Filters;
using Eventures.Web.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Eventures.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventsService eventsService;
        private readonly UserManager<User> userManager;
        private readonly ILogger logger;

        public EventsController(
            IEventsService eventsService,
            IOrderService orderService,
            UserManager<User> userManager,
            ILogger<EventsController> logger)
        {
            this.eventsService = eventsService;
            this.userManager = userManager;
            this.logger = logger;
        }

        [Authorize]
        public IActionResult All()
        {
            AllEventsViewModel[] events = this.eventsService.GetAllEvents()
                .Select(e => new AllEventsViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Start = FormatDateTime(e.Start),
                    End = FormatDateTime(e.End)
                })
                .ToArray();

            return this.View(events);
        }

        [Authorize]
        public IActionResult MyEvents()
        {
            MyEventsViewModel[] orders = this.eventsService.GetMyEvents(this.User.Identity.Name)
                .Select(o => new MyEventsViewModel
                {
                    Name = o.Event.Name,
                    Start = FormatDateTime(o.Event.Start),
                    End = FormatDateTime(o.Event.End),
                    Tickets = o.TicketsCount
                })
                .ToArray();

            return this.View(orders);
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

        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd-MMM-yy HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}