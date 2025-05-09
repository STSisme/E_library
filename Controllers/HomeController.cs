using E_Library.Data;
using Microsoft.AspNetCore.Mvc;

namespace E_Library.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

