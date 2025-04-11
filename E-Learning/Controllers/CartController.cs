using Microsoft.AspNetCore.Mvc;

namespace E_Learning.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
