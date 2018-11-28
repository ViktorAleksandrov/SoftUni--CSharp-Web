using System.Linq;
using CHUSHKA.Web.Services.Contracts;
using CHUSHKA.Web.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace CHUSHKA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View();
            }

            ProductsIndexViewModel[] products = this.productService.GetAllProducts().ToArray();

            return this.View(products);
        }
    }
}
