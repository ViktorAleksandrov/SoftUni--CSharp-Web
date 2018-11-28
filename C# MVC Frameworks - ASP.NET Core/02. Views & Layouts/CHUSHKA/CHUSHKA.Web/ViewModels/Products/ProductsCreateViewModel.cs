using System.ComponentModel.DataAnnotations;

namespace CHUSHKA.Web.ViewModels.Products
{
    public class ProductsCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
