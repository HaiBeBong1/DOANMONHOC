
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Tuan3.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
AddCookie(options =>
{
    options.Cookie.Name = "PetStoreCookie";
    options.LoginPath = "/User/Login";
});

builder.Services.AddSession();

var connectionString =
builder.Configuration.GetConnectionString("WebsiteBanHangConnection");
builder.Services.AddDbContext<WebsiteBanHangContext>(options =>
 options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{

    _ = endpoints.MapControllerRoute(
    name: "trang-chu",
    pattern: "trang-chu",
    defaults: new { controller = "Home", action = "Index" });

    _ = endpoints.MapControllerRoute(
    name: "lien-he",
    pattern: "lien-he",
    defaults: new { controller = "Contact", action = "Index" });


    _ = endpoints.MapControllerRoute(
     name: "bai-viet",
     pattern: "bai-viet",
     defaults: new { controller = "Article", action = "Index" });


    _ = endpoints.MapControllerRoute(
    name: "san-pham",
    pattern: "san-pham",
    defaults: new { controller = "Product", action = "Index" });

    _ = endpoints.MapControllerRoute(
     name: "dang-ky",
     pattern: "dang-ky",
     defaults: new { controller = "User", action = "Register" });

    _ = endpoints.MapControllerRoute(
    name: "dang-nhap",
    pattern: "dang-nhap",
    defaults: new { controller = "User", action = "Login" });



    _ = endpoints.MapControllerRoute(
     name: "thong-tin",
     pattern: "thong-tin",
     defaults: new { controller = "User", action = "Info" });

    _ = endpoints.MapControllerRoute(
     name: "dang-xuat",
     pattern: "dang-xuat",
     defaults: new { controller = "User", action = "Logout" });



    _ = endpoints.MapControllerRoute(
     name: "gio-hang",
     pattern: "gio-hang",
     defaults: new { controller = "Cart", action = "Index" });
   _ = endpoints.MapControllerRoute(
  name: "them-gio-hang",
  pattern: "them-gio-hang",
  defaults: new { controller = "Cart", action = "AddItem" });
    _= endpoints.MapControllerRoute(
 name: "thanh-toan",
 pattern: "thanh-toan",
 defaults: new { controller = "Cart", action = "Payment" });
    _ = endpoints.MapControllerRoute(
 name: "hoan-thanh",
 pattern: "hoan-thanh",
 defaults: new { controller = "Cart", action = "Success" });


    _ = endpoints.MapControllerRoute(
    name: "edit",
    pattern: "user/edit",
    defaults: new { controller = "User", action = "Edit" });

    _ = endpoints.MapControllerRoute(
    name: "the-loai-san-pham",
    pattern: "{slug}-{id}",
    defaults: new { controller = "Product", action = "CateProd" });

    _ = endpoints.MapControllerRoute(
    name: "chi-tiet-san-pham",
    pattern: "san-pham/{slug}-{id}",
    defaults: new { controller = "Product", action = "ProdDetail" });

    _ = endpoints.MapControllerRoute(
        name: "chuong-trinh",
 pattern: "chuong-trinh/{slug}",
 defaults: new { controller = "Product", action = "Index" });

    _ = endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();











