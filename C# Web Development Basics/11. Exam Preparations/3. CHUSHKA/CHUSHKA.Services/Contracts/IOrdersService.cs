using System.Collections.Generic;
using CHUSHKA.ViewModels.Orders;

namespace CHUSHKA.Services.Contracts
{
    public interface IOrdersService
    {
        IEnumerable<AllOrdersViewModel> GetAllOrders();
    }
}
