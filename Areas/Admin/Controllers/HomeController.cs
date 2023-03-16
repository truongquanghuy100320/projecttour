using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projecttour.Models;

namespace projecttour.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
      
        public IActionResult Index()
        {
            return View();
        }

     
    }
}
