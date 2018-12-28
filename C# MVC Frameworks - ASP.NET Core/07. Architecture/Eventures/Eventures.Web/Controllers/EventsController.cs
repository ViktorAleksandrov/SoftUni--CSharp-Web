using AutoMapper;
using Eventures.Models;
using Eventures.Services.Contracts;
using Eventures.Web.Filters;
using Eventures.Web.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace Eventures.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventsService eventsService;
        private readonly UserManager<User> userManager;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public EventsController(
            IEventsService eventsService,
            IOrdersService orderService,
            UserManager<User> userManager,
            ILogger<EventsController> logger,
            IMapper mapper)
        {
            this.eventsService = eventsService;
            this.userManager = userManager;
            this.logger = logger;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult All(int? page)
        {
            AllEventsViewModel[] events = this.mapper.Map<AllEventsViewModel[]>(this.eventsService.GetAllEvents());

            int pageNumber = page ?? 1;

            IPagedList<AllEventsViewModel> eventsOnPage = events.ToPagedList(pageNumber, 5);

            return this.View(eventsOnPage);
        }

        [Authorize]
        public IActionResult MyEvents()
        {
            MyEventsViewModel[] orders =
                this.mapper.Map<MyEventsViewModel[]>(this.eventsService.GetMyEvents(this.User.Identity.Name));

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
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            Event @event = this.mapper.Map<Event>(model);

            this.eventsService.CreateEvent(@event);

            this.logger.LogInformation($"Event created: {@event.Name}", @event);

            return this.RedirectToAction(nameof(All));
        }
    }
}