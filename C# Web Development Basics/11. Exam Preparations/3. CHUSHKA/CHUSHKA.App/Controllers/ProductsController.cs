using CHUSHKA.App.Controllers.Base;
using CHUSHKA.Models.Enums;
using CHUSHKA.Services.Contracts;
using CHUSHKA.ViewModels;
using CHUSHKA.ViewModels.Products;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;

namespace CHUSHKA.App.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            ProductsDetailsViewModel model = this.productsService.GetProduct(id);

            this.Model["Id"] = model.Id;
            this.Model["Name"] = model.Name;
            this.Model["Type"] = model.Type;
            this.Model["Price"] = model.Price;
            this.Model["Description"] = model.Description;

            return this.View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Order(int id)
        {
            this.productsService.OrderProduct(id, this.Identity.Username);

            return this.RedirectToAction("/");
        }

        [HttpGet]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Create(ProductsCreateViewModel model)
        {
            this.productsService.CreateProduct(model);

            return this.RedirectToAction("/");
        }

        [HttpGet]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Edit(int id)
        {
            ProductsDetailsViewModel model = this.productsService.GetProduct(id);

            this.Model["Id"] = model.Id;
            this.Model["Name"] = model.Name;
            this.Model["Price"] = model.Price;
            this.Model["Description"] = model.Description;
            this.SetProductType(model);

            return this.View();
        }

        [HttpPost]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Edit(ProductsDetailsViewModel model)
        {
            this.productsService.EditProduct(model);

            return this.RedirectToAction($"/Products/Details?id={model.Id}");
        }

        [HttpGet]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Delete(int id)
        {
            ProductsDetailsViewModel model = this.productsService.GetProduct(id);

            this.Model["Id"] = model.Id;
            this.Model["Name"] = model.Name;
            this.Model["Price"] = model.Price;
            this.Model["Description"] = model.Description;
            this.SetProductType(model);

            return this.View();
        }

        [HttpPost]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Delete(IdViewModel model)
        {
            this.productsService.DeleteProduct(model.Id);

            return this.RedirectToAction("/");
        }

        private void SetProductType(ProductsDetailsViewModel model)
        {
            switch (model.Type)
            {
                case "Food":
                    this.Model["Food"] = "checked";
                    break;
                case "Domestic":
                    this.Model["Domestic"] = "checked";
                    break;
                case "Health":
                    this.Model["Health"] = "checked";
                    break;
                case "Cosmetic":
                    this.Model["Cosmetic"] = "checked";
                    break;
                case "Other":
                    this.Model["Other"] = "checked";
                    break;
            }
        }
    }
}
