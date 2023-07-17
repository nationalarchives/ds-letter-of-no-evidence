using Microsoft.AspNetCore.Mvc;

namespace letter_of_no_evidence.web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
