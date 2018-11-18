using System;
using System.Globalization;
using System.Linq;
using PANDA.App.Data;
using PANDA.App.Models;
using PANDA.App.Models.Enums;
using PANDA.App.ViewModels.Packages;
using PANDA.App.ViewModels.Users;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;

namespace PANDA.App.Controllers
{
    public class PackagesController : BaseController
    {
        public PackagesController(PandaDbContext context)
            : base(context)
        {
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            PackagesDetailsViewModel package = this.context.Packages
                .Where(p => p.Id == id)
                .Select(p => new PackagesDetailsViewModel
                {
                    Address = p.ShippingAddress,
                    Status = p.Status.ToString(),
                    EstimatedDeliveryDate = this.GetEstimatedDeliveryDate(p.Status, p.EstimatedDeliveryDate),
                    Weight = p.Weight,
                    Recipient = p.Recipient.Username,
                    Description = p.Description
                })
                .FirstOrDefault();

            this.Model["Package"] = package;

            return this.View();
        }

        [HttpGet]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Create()
        {
            UserViewModel[] recipients = this.context.Users
                .Select(u => new UserViewModel
                {
                    Username = u.Username
                })
                .ToArray();

            this.Model["Recipients"] = recipients;

            return this.View();
        }

        [HttpPost]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Create(PackagesCreateViewModel model)
        {
            int recipientId = this.context.Users
                .FirstOrDefault(u => u.Username == model.Recipient)
                .Id;

            var package = new Package
            {
                Description = model.Description,
                Weight = model.Weight,
                ShippingAddress = model.ShippingAddress,
                RecipientId = recipientId
            };

            this.context.Packages.Add(package);
            this.context.SaveChanges();

            return this.RedirectToAction("/");
        }

        [HttpGet]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Pending()
        {
            PendingViewModel[] pending = this.context.Packages
                .Where(p => p.Status == Status.Pending)
                .Select(p => new PendingViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    Recipient = p.Recipient.Username
                })
                .ToArray();

            for (int i = 0; i < pending.Length; i++)
            {
                pending[i].Index = i + 1;
            }

            this.Model["Pending"] = pending;

            return this.View();
        }

        [HttpGet]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Shipped()
        {
            ShippedViewModel[] shipped = this.context.Packages
                .Where(p => p.Status == Status.Shipped)
                .Select(p => new ShippedViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight = p.Weight,
                    EstimatedDeliveryDate = this.GetEstimatedDeliveryDate(p.Status, p.EstimatedDeliveryDate),
                    Recipient = p.Recipient.Username
                })
                .ToArray();

            for (int i = 0; i < shipped.Length; i++)
            {
                shipped[i].Index = i + 1;
            }

            this.Model["Shipped"] = shipped;

            return this.View();
        }

        [HttpGet]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Delivered()
        {
            DeliveredViewModel[] delivered = this.context.Packages
                .Where(p => p.Status == Status.Delivered || p.Status == Status.Acquired)
                .Select(p => new DeliveredViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    Recipient = p.Recipient.Username
                })
                .ToArray();

            for (int i = 0; i < delivered.Length; i++)
            {
                delivered[i].Index = i + 1;
            }

            this.Model["Delivered"] = delivered;

            return this.View();
        }

        [HttpGet]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Ship(int id)
        {
            Package package = this.context.Packages.Find(id);

            int days = new Random().Next(20, 41);

            package.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(days);
            package.Status = Status.Shipped;

            this.context.SaveChanges();

            return this.RedirectToAction("/");
        }

        [HttpGet]
        [Authorize(nameof(Role.Admin))]
        public IActionResult Deliver(int id)
        {
            Package package = this.context.Packages.Find(id);

            package.Status = Status.Delivered;

            this.context.SaveChanges();

            return this.RedirectToAction("/");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Acquire(int id)
        {
            Package package = this.context.Packages.Find(id);

            package.Status = Status.Acquired;

            var receipt = new Receipt
            {
                Fee = package.Weight * 2.67m,
                RecipientId = package.RecipientId,
                PackageId = package.Id
            };

            this.context.Receipts.Add(receipt);
            this.context.SaveChanges();

            return this.RedirectToAction("/");
        }

        private string GetEstimatedDeliveryDate(Status status, DateTime? estimatedDeliveryDate)
        {
            switch (status)
            {
                case Status.Pending:
                    return "N/A";
                case Status.Shipped:
                    return estimatedDeliveryDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                default:
                    return "Delivered";
            }
        }
    }
}
