using System;
using System.Linq;
using Eventures.Data;
using Eventures.Models;
using Eventures.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Eventures.Tests
{
    public class OrdersServiceTests
    {
        [Fact]
        public void CreateOrder_ShouldReduceTicketsCountAndSaveOrderInDb()
        {
            DbContextOptions options = new DbContextOptionsBuilder<EventuresDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new EventuresDbContext(options);
            var eventsService = new EventsService(dbContext);
            var ordersService = new OrdersService(dbContext, eventsService);

            var user = new User { Id = "123" };
            var @event = new Event { Id = "456", TotalTickets = 10 };

            dbContext.Users.Add(user);
            dbContext.Events.Add(@event);
            dbContext.SaveChanges();

            var order = new Order
            {
                CustomerId = user.Id,
                EventId = @event.Id,
                TicketsCount = 2
            };

            ordersService.CreateOrder(order);

            Order dbOrder = dbContext.Orders.Single();

            Assert.True(dbOrder.CustomerId == user.Id);
            Assert.True(dbOrder.EventId == @event.Id);
            Assert.True(@event.TotalTickets == 8);
        }

        [Fact]
        public void GetAllOrders_ShouldReturn_AllOrdersIncludingCustomerAndEvent()
        {
            DbContextOptions options = new DbContextOptionsBuilder<EventuresDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new EventuresDbContext(options);
            var ordersService = new OrdersService(dbContext, null);

            Order[] orders = new[]
            {
                new Order
                {
                    Customer = new User { UserName = "Pesho" },
                    Event = new Event { Name = "Workshop" },
                    TicketsCount = 3
                },

                new Order
                {
                    Customer = new User { UserName = "Gosho" },
                    Event = new Event { Name = "Lab" },
                    TicketsCount = 10
                }
            };

            dbContext.Orders.AddRange(orders);
            dbContext.SaveChanges();

            Order[] dbOrders = ordersService.GetAllOrders()
                .OrderBy(o => o.TicketsCount)
                .ToArray();

            Assert.True(dbOrders.First().Customer.UserName == "Pesho");
            Assert.True(dbOrders.Last().Event.Name == "Lab");
            Assert.True(dbOrders.Count() == 2);
        }
    }
}
