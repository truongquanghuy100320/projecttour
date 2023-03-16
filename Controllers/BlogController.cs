using Microsoft.AspNetCore.Mvc;

namespace projecttour.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
