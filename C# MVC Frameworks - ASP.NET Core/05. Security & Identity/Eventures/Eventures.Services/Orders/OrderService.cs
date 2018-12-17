using System.Collections.Generic;
using System.Linq;
using Eventures.Data;
using Eventures.Models;
using Eventures.Services.Orders.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Eventures.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly EventuresDbContext dbContext;

        public OrderService(EventuresDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateOrder(Order order)
        {
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
