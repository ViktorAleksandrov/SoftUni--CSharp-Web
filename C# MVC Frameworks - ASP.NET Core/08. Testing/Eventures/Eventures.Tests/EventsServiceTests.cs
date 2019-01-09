using System;
using System.Linq;
using Eventures.Data;
using Eventures.Models;
using Eventures.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Eventures.Tests
{
    public class EventsServiceTests
    {
        [Fact]
        public void CreateEvent_ShouldSaveEventInDb()
        {
            DbContextOptions options = new DbContextOptionsBuilder<EventuresDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new EventuresDbContext(options);
            var eventsService = new EventsService(dbContext);

            var @event = new Event
            {
                Name = "Workshop",
                Place = "Varna",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddDays(1),
                TicketPrice = 1,
                TotalTickets = 2
            };

            eventsService.CreateEvent(@event);

            Event dbEvent = dbContext.Events.Single();

            Assert.True(dbEvent.Name == "Workshop");
            Assert.True(dbEvent.Place == "Varna");
            Assert.True(dbEvent.TicketPrice == 1);
            Assert.True(dbEvent.TotalTickets == 2);
        }

        [Fact]
        public void GetAllEventsWithAvailableTickets_ShouldReturn_AllEventsWithAtLeastOneAvailableTicket()
        {
            DbContextOptions options = new DbContextOptionsBuilder<EventuresDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new EventuresDbContext(options);
            var eventsService = new EventsService(dbContext);

            for (int i = 1; i <= 10; i++)
            {
                var @event = new Event
                {
                    Name = $"Workshop {i}",
                    Place = "Varna",
                    Start = DateTime.UtcNow,
                    End = DateTime.UtcNow.AddDays(i),
                    TicketPrice = 1 * i,
                    TotalTickets = 4 - i
                };

                dbContext.Events.Add(@event);
            }

            dbContext.SaveChanges();

            int eventsCount = eventsService.GetAllEventsWithAvailableTickets().Count();

            Assert.True(eventsCount == 3);
        }

        [Fact]
        public void GetEventById_WithValidId_ShouldReturn_CorrectEvent()
        {
            DbContextOptions options = new DbContextOptionsBuilder<EventuresDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new EventuresDbContext(options);
            var eventsService = new EventsService(dbContext);

            for (int i = 1; i <= 10; i++)
            {
                var @event = new Event
                {
                    Id = $"{i}",
                    Name = $"Workshop {i}",
                    Place = "Varna",
                    Start = DateTime.UtcNow,
                    End = DateTime.UtcNow.AddDays(i),
                    TicketPrice = 1 * i,
                    TotalTickets = 2 * i
                };

                dbContext.Events.Add(@event);
            }

            dbContext.SaveChanges();

            Event testEvent = eventsService.GetEventById("3");
            Event dbEvent = dbContext.Events.Find("3");

            Assert.Same(testEvent, dbEvent);
        }

        [Fact]
        public void GetEventById_WithInvalidId_ShouldReturn_Null()
        {
            DbContextOptions options = new DbContextOptionsBuilder<EventuresDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new EventuresDbContext(options);
            var eventsService = new EventsService(dbContext);

            for (int i = 1; i <= 10; i++)
            {
                var @event = new Event
                {
                    Id = $"{i}",
                    Name = $"Workshop {i}",
                    Place = "Varna",
                    Start = DateTime.UtcNow,
                    End = DateTime.UtcNow.AddDays(i),
                    TicketPrice = 1 * i,
                    TotalTickets = 2 * i
                };

                dbContext.Events.Add(@event);
            }

            dbContext.SaveChanges();

            Event testEvent = eventsService.GetEventById("22");

            Assert.Null(testEvent);
        }

        [Fact]
        public void GetMyEvents_ShouldReturn_OrdersCreatedByUser()
        {
            DbContextOptions options = new DbContextOptionsBuilder<EventuresDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new EventuresDbContext(options);
            var eventsService = new EventsService(dbContext);

            User[] users = new[]
            {
                new User
                {
                    UserName = "Pesho"
                },

                new User
                {
                    UserName = "Gosho"
                }
            };

            Order[] orders = new[]
            {
                new Order
                {
                    Customer = users[0],
                    EventId = "1",
                    TicketsCount = 1
                },

                new Order
                {
                    Customer = users[0],
                    EventId = "2",
                    TicketsCount = 2
                },

                new Order
                {
                    Customer = users[1],
                    EventId = "3",
                    TicketsCount = 5
                }
            };

            dbContext.Orders.AddRange(orders);
            dbContext.SaveChanges();

            int peshoOrdersCount = eventsService.GetMyEvents(users[0].UserName).Count();

            Assert.True(peshoOrdersCount == 2);
        }
    }
}
