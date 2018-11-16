using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CHUSHKA.Data;
using CHUSHKA.Services.Base;
using CHUSHKA.Services.Contracts;
using CHUSHKA.ViewModels.Orders;

namespace CHUSHKA.Services
{
    public class OrdersService : BaseService, IOrdersService
    {
        public OrdersService(ChushkaDbContext context)
            : base(context)
        {
        }

        public IEnumerable<AllOrdersViewModel> GetAllOrders()
        {
            AllOrdersViewModel[] orders = this.context.Orders
                .Select(o => new AllOrdersViewModel
                {
                    Id = o.Id,
                    Customer = o.Client.Username,
                    Product = o.Product.Name,
                    OrderedOn = o.OrderedOn.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture)
                })
                .ToArray();

            for (int i = 0; i < orders.Length; i++)
            {
                orders[i].Index = i + 1;
            }

            return orders;
        }
    }
}
