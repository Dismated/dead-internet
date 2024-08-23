using Microsoft.AspNetCore.Mvc;

namespace LLMForum.Server.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
