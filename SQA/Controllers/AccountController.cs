using Microsoft.AspNetCore.Mvc;

namespace SQA.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied(string ReturnUrl) => View();
    }
}