using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuan3.Models;
using Tuan3.ViewModels;

namespace Tuan3.Controllers
{
    public class ArticleController : Controller
    {
        private readonly WebsiteBanHangContext _context;

        public ArticleController(WebsiteBanHangContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
            var product1Title = "Kem chống nắng Caryophy";
            var product1Image = "~/images/products/444_5a0f19af86a44bd4995c025a22d7c2fc_1024x1024_16126ba0293840ac8403170e96d54c75_1024x1024.webp";
            var product1Content = "Nội dung bài viết về sản phẩm chống nắng Caryophy...";

            var product2Title = "Sữa rửa mặt SVR";
            var product2Image = "~/images/products/sg-11134201-22120-gd89993r5ukv05_copy_b6a28d4077dc4f5abd47d32133714466.webp";
            var product2Content = "Nội dung bài viết về sản phẩm sữa rửa mặt SVR...";

        

            var viewModel = new ArticleViewModel

            {
                Menus = menus,
                Blogs = blogs,
                Product1Title = product1Title,
                Product1Image = product1Image,
                Product1Content = product1Content,
                Product2Title = product2Title,
                Product2Image = product2Image,
                Product2Content = product2Content,

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