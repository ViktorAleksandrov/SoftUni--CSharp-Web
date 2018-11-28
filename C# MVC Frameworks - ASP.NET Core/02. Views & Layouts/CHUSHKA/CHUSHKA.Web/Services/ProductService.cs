using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CHUSHKA.Data;
using CHUSHKA.Models;
using CHUSHKA.Models.Enums;
using CHUSHKA.Web.Services.Contracts;
using CHUSHKA.Web.ViewModels.Products;

namespace CHUSHKA.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly ChushkaDbContext db;

        public ProductService(ChushkaDbContext db)
        {
            this.db = db;
        }

        public void CreateProduct(ProductsCreateViewModel model)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Type = Enum.Parse<ProductType>(model.Type)
            };

            this.db.Products.Add(product);
            this.db.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            Product product = this.db.Products.Find(id);

            if (product != null)
            {
                this.db.Products.Remove(product);
                this.db.SaveChanges();
            }
        }

        public void EditProduct(ProductsEditDeleteViewModel model)
        {
            Product product = this.db.Products
                .FirstOrDefault(p => p.Id == model.Id);

            if (product != null)
            {
                product.Name = model.Name;
                product.Price = model.Price;
                product.Description = model.Description;
                product.Type = Enum.Parse<ProductType>(model.Type);

                this.db.SaveChanges();
            }
        }

        public IEnumerable<ProductsIndexViewModel> GetAllProducts()
        {
            ProductsIndexViewModel[] products = this.db.Products
                .Select(p => new ProductsIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description.Length > 50
                        ? p.Description.Substring(0, 50) + "..."
                        : p.Description,
                    Price = p.Price.ToString().Contains(".")
                        ? p.Price.ToString().TrimEnd('0').TrimEnd('.')
                        : p.Price.ToString()
                })
                .ToArray();

            return products;
        }

        public Product GetProduct(int id)
        {
            return this.db.Products.Find(id);
        }

        public void OrderProduct(int productId, string username)
        {
            Product product = this.db.Products.Find(productId);
            User client = this.db.Users.FirstOrDefault(u => u.UserName == username);

            if (product != null && client != null)
            {
                var order = new Order
                {
                    ProductId = productId,
                    ClientId = client.Id
                };

                this.db.Orders.Add(order);
                this.db.SaveChanges();
            }
        }

        public IEnumerable<AllOrdersViewModel> GetAllOrders()
        {
            AllOrdersViewModel[] orders = this.db.Orders
                .Select(o => new AllOrdersViewModel
                {
                    Id = o.Id,
                    Customer = o.Client.UserName,
                    Product = o.Product.Name,
                    OrderedOn = o.OrderedOn.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture)
                })
                .ToArray();

            return orders;
        }
    }
}
