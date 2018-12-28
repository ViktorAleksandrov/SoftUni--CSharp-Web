using System.Linq;
using Eventures.Models;

namespace Eventures.Services.Contracts
{
    public interface IOrdersService
    {
        void CreateOrder(Order order);

        IQueryable<Order> GetAllOrders();
    }
}
