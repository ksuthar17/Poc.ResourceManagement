using Microsoft.AspNetCore.Mvc;

namespace Poc.ResourceManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
