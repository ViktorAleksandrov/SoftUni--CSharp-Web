using System.Collections.Generic;
using System.Linq;
using Eventures.Data;
using Eventures.Models;
using Eventures.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Eventures.Services
{
    public class OrderService : IOrderService
    {
        private readonly EventuresDbContext dbContext;
        private readonly IEventsService eventsService;

        public OrderService(EventuresDbContext dbContext, IEventsService eventsService)
        {
            this.dbContext = dbContext;
            this.eventsService = eventsService;
        }

        public void CreateOrder(Order order)
        {
            Event @event = this.eventsService.GetEventById(order.EventId);
            @event.TotalTickets -= order.TicketsCount;

            this.dbContext.Orders.Add(order);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return this.dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Event)
                .ToArray();
        }
    }
}
