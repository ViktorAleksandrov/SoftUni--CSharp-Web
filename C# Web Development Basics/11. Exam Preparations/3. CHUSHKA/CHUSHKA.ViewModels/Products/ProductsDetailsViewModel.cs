using CHUSHKA.ViewModels.Products.Base;

namespace CHUSHKA.ViewModels.Products
{
    public class ProductsDetailsViewModel : ProductsBaseViewModel
    {
        public int Id { get; set; }

        public string Type { get; set; }
    }
}
