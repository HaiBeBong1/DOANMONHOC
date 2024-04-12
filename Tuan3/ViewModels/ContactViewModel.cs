using Tuan3.Models;
using System.Collections.Generic;
namespace Tuan3.ViewModels
{
    public class ContactViewModel
    {
        public List<Menu> Menus { get; set; } // Danh sách menu
        public List<Blog> Blogs { get; set; } // Danh sách blog
        public List<Product> Prods { get; set; }

    }
}
