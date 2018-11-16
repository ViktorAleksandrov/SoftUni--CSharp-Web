using CHUSHKA.ViewModels.Products.Base;

namespace CHUSHKA.ViewModels.Products
{
    public class ProductsIndexViewModel : ProductsBaseViewModel
    {
        public int Id { get; set; }

        public string ShortDescription
            => this.Description.Length > 50 ? this.Description.Substring(0, 50) + "..." : this.Description;
    }
}
