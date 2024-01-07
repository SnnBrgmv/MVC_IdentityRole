using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P335_BackEnd.Data;
using P335_BackEnd.Models;
using System.Diagnostics;

namespace P335_BackEnd.Controllers
{
	public class HomeController : Controller
	{
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(int? categoryId)
		{
            var categories = _dbContext.Categories.Include(x=>x.CategoryImage).ToList();
            if (categories is null) return NotFound();

            var featured = _dbContext.ProductTypesProducts
                .Include(x=>x.ProductType)
                .Include(x=>x.Product)
                .Where(x=> x.ProductType.Name == "Featured" && (categoryId == null ? true : x.Product.CategoryId == categoryId))
                .Select(x=>x.Product)
                .ToList();

            if (featured is null) return NotFound();

            var latest = _dbContext.ProductTypesProducts
                .Include(x => x.ProductType)
                .Include(x => x.Product)
                .Where(x => x.ProductType.Name == "Latest")
                .Select(x => x.Product)
                .ToList();

            if (latest is null) return NotFound();

            var topRated = _dbContext.ProductTypesProducts
                .Include(x => x.ProductType)
                .Include(x => x.Product)
                .Where(x => x.ProductType.Name == "Top Rated")
                .Select(x => x.Product)
                .ToList();

            if (topRated is null) return NotFound();

            var review = _dbContext.ProductTypesProducts
                .Include(x => x.ProductType)
                .Include(x => x.Product)
                .Where(x => x.ProductType.Name == "Review")
                .Select(x => x.Product)
                .ToList();

            if (review is null) return NotFound();

            var model = new HomeIndexVM
            {
                Categories = categories,
                FeaturedProducts = featured,
                LatestProducts = latest,
                TopRatedProducts = topRated,
                ReviewProducts = review
            };
			return View(model);
		}
	}
}
