using System.Globalization;
using System.Linq;
using PANDA.App.Data;
using PANDA.App.ViewModels.Receipts;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;

namespace PANDA.App.Controllers
{
    public class ReceiptsController : BaseController
    {
        public ReceiptsController(PandaDbContext context)
            : base(context)
        {
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            ReceiptsIndexViewModel[] receipts = this.context.Receipts
                .Where(r => r.Recipient.Username == this.Identity.Username)
                .Select(r => new ReceiptsIndexViewModel
                {
                    Id = r.Id,
                    Fee = r.Fee,
                    IssuedOn = r.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Recipient = r.Recipient.Username
                })
                .ToArray();

            this.Model["Receipts"] = receipts;

            return this.View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            ReceiptsDetailsViewModel receipt = this.context.Receipts
                .Where(r => r.Id == id && r.Recipient.Username == this.Identity.Username)
                .Select(r => new ReceiptsDetailsViewModel
                {
                    Id = r.Id,
                    IssuedOn = r.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DeliveryAddress = r.Package.ShippingAddress,
                    Weight = r.Package.Weight,
                    Description = r.Package.Description,
                    Recipient = r.Recipient.Username,
                    Fee = r.Fee
                })
                .FirstOrDefault();

            this.Model["Receipt"] = receipt;

            return this.View();
        }
    }
}
