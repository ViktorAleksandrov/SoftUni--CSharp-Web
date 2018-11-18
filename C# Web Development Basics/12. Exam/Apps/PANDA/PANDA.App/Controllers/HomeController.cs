using System.Linq;
using Microsoft.EntityFrameworkCore;
using PANDA.App.Data;
using PANDA.App.Models;
using PANDA.App.Models.Enums;
using PANDA.App.ViewModels.Packages;
using SIS.Framework.ActionResults;

namespace PANDA.App.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(PandaDbContext context)
            : base(context)
        {
        }

        public IActionResult Index()
        {
            if (this.IsLoggedIn)
            {
                User user = this.context.Users.Include(u => u.Packages)
                    .FirstOrDefault(u => u.Username == this.Identity.Username);

                IndexPackagesViewModel[] pending = user.Packages
                    .Where(p => p.Status == Status.Pending)
                    .Select(p => new IndexPackagesViewModel
                    {
                        Id = p.Id,
                        Description = p.Description
                    })
                    .ToArray();

                IndexPackagesViewModel[] shipped = user.Packages
                    .Where(p => p.Status == Status.Shipped)
                    .Select(p => new IndexPackagesViewModel
                    {
                        Id = p.Id,
                        Description = p.Description
                    })
                    .ToArray();

                IndexDeliveredViewModel[] delivered = user.Packages
                    .Where(p => p.Status == Status.Delivered)
                    .Select(p => new IndexDeliveredViewModel
                    {
                        Id = p.Id,
                        Description = p.Description
                    })
                    .ToArray();

                this.Model["Username"] = this.Identity.Username;
                this.Model["Pending"] = pending;
                this.Model["Shipped"] = shipped;
                this.Model["Delivered"] = delivered;
            }

            return this.View();
        }
    }
}
