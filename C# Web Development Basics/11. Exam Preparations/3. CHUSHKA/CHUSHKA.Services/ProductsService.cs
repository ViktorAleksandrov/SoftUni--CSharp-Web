using System;
using System.Collections.Generic;
using System.Linq;
using CHUSHKA.Data;
using CHUSHKA.Models;
using CHUSHKA.Models.Enums;
using CHUSHKA.Services.Base;
using CHUSHKA.Services.Contracts;
using CHUSHKA.ViewModels.Products;

namespace CHUSHKA.Services
{
    public class ProductsService : BaseService, IProductsService
    {
        public ProductsService(ChushkaDbContext context)
            : base(context)
        {
        }

        public void CreateProduct(ProductsCreateViewModel model)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = decimal.Parse(model.Price),
                Description = model.Description,
                Type = Enum.Parse<ProductType>(model.Type)
            };

            this.context.Products.Add(product);
            this.context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            Product product = this.context.Products.Find(id);

            product.IsDeleted = true;

            this.context.SaveChanges();
        }

        public void EditProduct(ProductsDetailsViewModel model)
        {
            Product product = this.context.Products
                .FirstOrDefault(p => p.Id == model.Id && !p.IsDeleted);

            if (product != null)
            {
                product.Name = model.Name;
                product.Price = decimal.Parse(model.Price);
                product.Description = model.Description;
                product.Type = Enum.Parse<ProductType>(model.Type);

                this.context.SaveChanges();
            }
        }

        public IEnumerable<ProductsIndexViewModel> GetAllProducts()
        {
            ProductsIndexViewModel[] products = this.context.Products
                .Where(p => !p.IsDeleted)
                .Select(p => new ProductsIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price.ToString().Contains(".")
                        ? p.Price.ToString().TrimEnd('0').TrimEnd('.')
                        : p.Price.ToString()
                })
                .ToArray();

            return products;
        }

        public ProductsDetailsViewModel GetProduct(int id)
        {
            ProductsDetailsViewModel product = this.context.Products
                .Where(p => p.Id == id && !p.IsDeleted)
                .Select(p => new ProductsDetailsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type.ToString(),
                    Price = p.Price.ToString().Contains(".")
                        ? p.Price.ToString().TrimEnd('0').TrimEnd('.')
                        : p.Price.ToString(),
                    Description = p.Description
                })
                .FirstOrDefault();

            return product;
        }

        public void OrderProduct(int productId, string username)
        {
            int clientId = this.context.Users
                .FirstOrDefault(u => u.Username == username)
                .Id;

            var order = new Order
            {
                ProductId = productId,
                ClientId = clientId
            };

            this.context.Orders.Add(order);
            this.context.SaveChanges();
        }
    }
}
