using System.Collections.Generic;
using CHUSHKA.ViewModels.Products;

namespace CHUSHKA.Services.Contracts
{
    public interface IProductsService
    {
        IEnumerable<ProductsIndexViewModel> GetAllProducts();

        ProductsDetailsViewModel GetProduct(int id);

        void OrderProduct(int productId, string username);

        void CreateProduct(ProductsCreateViewModel model);

        void EditProduct(ProductsDetailsViewModel model);

        void DeleteProduct(int id);
    }
}
