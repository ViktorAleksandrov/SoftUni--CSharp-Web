using System.Linq;
using Eventures.Models;
using Eventures.Services.Events.Contracts;
using Eventures.Services.Orders.Contracts;
using Eventures.Web.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Eventures.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IEventsService eventsService;
        private readonly UserManager<User> userManager;

        public OrdersController(IOrderService orderService, IEventsService eventsService, UserManager<User> userManager)
        {
            this.orderService = orderService;
            this.eventsService = eventsService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateOrderBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (!this.eventsService.EventExists(model.EventId))
                {
                    return this.RedirectToAction("All", "Events");
                }

                string userId = this.userManager.GetUserId(this.User);

                var order = new Order
                {
                    CustomerId = userId,
                    EventId = model.EventId,
                    TicketsCount = model.Tickets
                };

                this.orderService.CreateOrder(order);

                return this.RedirectToAction("MyEvents", "Events");
            }

            return this.RedirectToAction("All", "Events");
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult All()
        {
            AllOrdersViewModel[] orders = this.orderService.GetAllOrders()
                 .Select(o => new AllOrdersViewModel
                 {
                     Event = o.Event.Name,
                     Customer = o.Customer.UserName,
                     OrderedOn = EventsController.FormatDateTime(o.OrderedOn)
                 })
                 .ToArray();

            return this.View(orders);
        }
    }
}
