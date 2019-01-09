using AutoMapper;
using Eventures.Models;
using Eventures.Services.Contracts;
using Eventures.Web.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventures.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService ordersService;
        private readonly IEventsService eventsService;
        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        public OrdersController(
            IOrdersService ordersService, IEventsService eventsService, IAccountService accountService, IMapper mapper)
        {
            this.ordersService = ordersService;
            this.eventsService = eventsService;
            this.accountService = accountService;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateOrderBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("All", "Events");
            }

            Event @event = this.eventsService.GetEventById(model.EventId);

            if (@event == null)
            {
                return this.RedirectToAction("All", "Events");
            }

            int availableTickets = @event.TotalTickets;

            if (availableTickets < model.TicketsCount)
            {
                this.TempData["Error"] =
                    $"There are not enough tickets! The number of available tickets is {availableTickets}.";

                return this.RedirectToAction("All", "Events");
            }

            string userId = this.accountService.GetUserId(this.User);

            Order order = this.mapper.Map<Order>(model);
            order.CustomerId = userId;

            this.ordersService.CreateOrder(order);

            return this.RedirectToAction("MyEvents", "Events");
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult All()
        {
            AllOrdersViewModel[] orders = this.mapper.Map<AllOrdersViewModel[]>(this.ordersService.GetAllOrders());

            return this.View(orders);
        }
    }
}
