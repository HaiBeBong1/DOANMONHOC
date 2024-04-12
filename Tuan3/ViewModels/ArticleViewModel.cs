using Tuan3.Models;

namespace Tuan3.ViewModels
{
    public class ArticleViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<Blog> Blogs { get; set; }
        public string Product1Title { get; set; }
        public string Product1Image { get; set; }
        public string Product1Content { get; set; }

        public string Product2Title { get; set; }
        public string Product2Image { get; set; }
        public string Product2Content { get; set; }

    }
}
