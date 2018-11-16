using CHUSHKA.App.Controllers.Base;
using CHUSHKA.Models.Enums;
using CHUSHKA.Services.Contracts;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;

namespace CHUSHKA.App.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [HttpGet]
        [Authorize(nameof(Role.Admin))]
        public IActionResult All()
        {
            this.Model["Orders"] = this.ordersService.GetAllOrders();

            return this.View();
        }
    }
}
