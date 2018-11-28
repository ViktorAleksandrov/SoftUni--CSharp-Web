using System.Collections.Generic;
using CHUSHKA.Models;
using CHUSHKA.Web.ViewModels.Products;

namespace CHUSHKA.Web.Services.Contracts
{
    public interface IProductService
    {
        IEnumerable<ProductsIndexViewModel> GetAllProducts();

        Product GetProduct(int id);

        void OrderProduct(int productId, string username);

        void CreateProduct(ProductsCreateViewModel model);

        void EditProduct(ProductsEditDeleteViewModel model);

        void DeleteProduct(int id);

        IEnumerable<AllOrdersViewModel> GetAllOrders();
    }
}
