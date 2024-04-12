using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuan3.Models;
using Tuan3.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Tuan3.Models;
using Tuan3.ViewModels;

namespace Tuan3.Controllers
{
    public class ContactController : Controller
    {
        private readonly WebsiteBanHangContext _context;

        public ContactController(WebsiteBanHangContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
           
            var viewModel = new ContactViewModel
            
            {
                Menus = menus,
                Blogs = blogs,
              

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