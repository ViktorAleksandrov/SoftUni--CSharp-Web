using System.Collections.Generic;
using Eventures.Models;

namespace Eventures.Services.Orders.Contracts
{
    public interface IOrderService
    {
        void CreateOrder(Order order);

        IEnumerable<Order> GetAllOrders();
    }
}
