using System.Linq;
using CHUSHKA.Models;
using CHUSHKA.Web.Services.Contracts;
using CHUSHKA.Web.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CHUSHKA.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService, UserManager<User> userManager)
        {
            this.productService = productService;
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            Product product = this.productService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            var model = new ProductsDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Type = product.Type.ToString(),
                Price = product.Price.ToString().Contains(".")
                        ? product.Price.ToString().TrimEnd('0').TrimEnd('.')
                        : product.Price.ToString(),
                Description = product.Description
            };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Order(int id)
        {
            string username = this.User.Identity.Name;

            this.productService.OrderProduct(id, username);

            return this.Redirect("/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductsCreateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            this.productService.CreateProduct(model);

            return this.Redirect("/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Product product = this.productService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            var model = new ProductsEditDeleteViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Type = product.Type.ToString(),
                Price = product.Price.ToString().Contains(".")
                        ? decimal.Parse(product.Price.ToString().TrimEnd('0').TrimEnd('.'))
                        : product.Price,
                Description = product.Description
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(ProductsEditDeleteViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.productService.EditProduct(model);

            return this.RedirectToAction("Details", "Products", new { id = model.Id });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Product product = this.productService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            var model = new ProductsEditDeleteViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Type = product.Type.ToString(),
                Price = product.Price.ToString().Contains(".")
                        ? decimal.Parse(product.Price.ToString().TrimEnd('0').TrimEnd('.'))
                        : product.Price,
                Description = product.Description
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ActionName("Delete")]
        public IActionResult DoDelete(int id)
        {
            this.productService.DeleteProduct(id);

            return this.Redirect("/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult All()
        {
            AllOrdersViewModel[] orders = this.productService.GetAllOrders().ToArray();

            return this.View(orders);
        }
    }
}