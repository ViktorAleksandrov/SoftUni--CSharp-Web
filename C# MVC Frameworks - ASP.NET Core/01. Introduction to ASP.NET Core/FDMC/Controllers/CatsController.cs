using System.Linq;
using FDMC.Data;
using FDMC.Models;
using FDMC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FDMC.Controllers
{
    public class CatsController : Controller
    {
        private readonly FdmcDbContext db;

        public CatsController(FdmcDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            IndexCatViewModel[] cats = this.db.Cats
                .Select(c => new IndexCatViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToArray();

            return this.View(cats);
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Add(CatViewModel model)
        {
            var cat = new Cat
            {
                Name = model.Name,
                Age = model.Age,
                Breed = model.Breed,
                ImageUrl = model.ImageUrl
            };

            this.db.Cats.Add(cat);
            this.db.SaveChanges();

            return this.Redirect("/");
        }

        [HttpGet("/cats/{id:int}")]
        public IActionResult Details(int id)
        {
            CatViewModel cat = this.db.Cats
                .Where(c => c.Id == id)
                .Select(c => new CatViewModel
                {
                    Name = c.Name,
                    Age = c.Age,
                    Breed = c.Breed,
                    ImageUrl = c.ImageUrl
                })
                .FirstOrDefault();

            return this.View(cat);
        }
    }
}