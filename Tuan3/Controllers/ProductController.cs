using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuan3.Models;
using Tuan3.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Tuan3.Controllers
{
    public class ProductController : Controller
    {
        private readonly WebsiteBanHangContext _context;

        public ProductController(WebsiteBanHangContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
            var prods = await _context.Products.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();

            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods,
            };

            return View(viewModel);
        }


        public async Task<IActionResult> CateProd(string slug, long id)
        {
            var cateProds = await _context.Catologies.FirstOrDefaultAsync(cp => cp.IdCat == id && cp.Link == slug);

            if (cateProds == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = "CateProd Error",
                };
                return View("Error", errorViewModel);
            }

            var prods = await _context.Products.Where(m => m.Hide == 0 && m.IdCat == cateProds.IdCat).OrderBy(m => m.Order).ToListAsync();

            var viewModel = new ProductViewModel
            {
                Menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync(),
                Prods = prods,
                cateName = cateProds.NameCat,
            };

            return View(viewModel);
        }
        public async Task<IActionResult> ProdDetail(string slug, long id)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).Take(2).ToListAsync();
            var prods = await _context.Products.Where(m => m.Link == slug && m.IdPro ==
           id).ToListAsync();
            if (prods == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = "Product Error",
                };
                return View("Error", errorViewModel);
            }
            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods,
            };
            return View(viewModel);
        }

        public async Task<IActionResult> _MenuPartial()
        {
            // Đoạn code của bạn cho _MenuPartial
            return PartialView();
        }

        public async Task<IActionResult> _BlogPartial()
        {
            // Đoạn code của bạn cho _BlogPartial
            return PartialView();
        }
    }
}
