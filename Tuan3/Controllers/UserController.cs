using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tuan3.Models;
using Tuan3.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Tuan3.Controllers
{
    public class UserController : Controller
    {
        private readonly WebsiteBanHangContext _context;
        public UserController(WebsiteBanHangContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).Take(2).ToListAsync();
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).Take(2).ToListAsync();
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Register = model.Register,
            };
            if (model.Register != null)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username ==
               model.Register.Username);
                if (existingUser != null)
                {
                   ViewBag.ErrorMessage = "Tên đăng nhập đã tồn tại.";
                    return View(viewModel);
                }
                model.Register.Password =
               BCrypt.Net.BCrypt.HashPassword(model.Register.Password);
                model.Register.Permission = 0;
                model.Register.Hide = 0;
                _context.Users.Add(model.Register);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "User");
            }
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).Take(2).ToListAsync();
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Register = model.Register,
            };
            if (model.Register != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username ==
               model.Register.Username);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Register.Password,
               user.Password))
                {
                    var claims = new List<Claim>
                     {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Permission.ToString()),
                    };
                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                    };
                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).Take(2).ToListAsync();
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
            };
            return View(viewModel);
        }
        public async Task<IActionResult> _MenuPartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> _BlogPartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> Info()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
           m.Order).Take(2).ToListAsync();
            var users = new User();
            if (User.Identity.IsAuthenticated)
            {
                string username = User.Identity.Name;
                if (username != null)
                {
                    users = await _context.Users.FirstOrDefaultAsync(m => m.Username ==
                   username);
                }
            }
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Register = users,
            };
            return View(viewModel);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();

            // Lấy thông tin người dùng từ cơ sở dữ liệu hoặc từ bất kỳ nguồn dữ liệu nào khác
            // Ví dụ: Lấy thông tin người dùng hiện tại từ HttpContext.User
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

            // Tạo một đối tượng UserViewModel và gán thông tin từ currentUser vào đó
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Register = currentUser
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // Chỉ cho phép người dùng đã đăng nhập truy cập
        public async Task<IActionResult> Edit(UserViewModel model)
        {
           /*  Kiểm tra ModelState.IsValid để đảm bảo dữ liệu được nhập vào form là hợp lệ
           if (model.Register.Username != User.Identity.Name)
            {
                 Nếu có, trả về thông báo lỗi
               ViewBag.ErrorMessage = "Không thể thay đổi tên đăng nhập.";
                return View(model);
            }
           

             Chuyển hướng người dùng đến trang thông tin sau khi chỉnh sửa thành công
          
           if (ModelState.IsValid)
            {
               
               Chuyển hướng người dùng đến trang thông tin sau khi chỉnh sửa thành công
              return RedirectToAction("Info");
           }
           /*/
           
            // Nếu dữ liệu không hợp lệ, trả về view chỉnh sửa với thông tin nhập vào và thông báo lỗi
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Register = model.Register
            };

            return View(viewModel);

        }

       
     }
    

}
