using Tuan3.Models;

namespace Tuan3.ViewModels
{
    public class CartViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<CartItem> CartItems { get; set; }
    }

}
