using Microsoft.AspNetCore.Mvc;

namespace ShelkovyPut_Main.Controllers.Setting
{
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            ViewBag.UserEmail = User.Identity.Name;            
            ViewBag.AdminID = "ea00f792-2d03-4f79-9bae-2730bc26d5fa";
            return View();
        }

        public IActionResult UserChat()
        {
            ViewBag.UserEmail = User.Identity.Name;
            ViewBag.AdminID = "ea00f792-2d03-4f79-9bae-2730bc26d5fa";
            return View();
        }
    }
}
