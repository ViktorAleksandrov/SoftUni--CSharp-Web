using CHUSHKA.App.Controllers.Base;
using CHUSHKA.Services.Contracts;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Method;

namespace CHUSHKA.App.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductsService productsService;

        public HomeController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (this.IsLoggedIn)
            {
                this.Model["Username"] = this.Identity.Username;

                this.Model["Products"] = this.productsService.GetAllProducts();
            }

            return this.View();
        }
    }
}
