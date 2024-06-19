using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        private readonly Library_Management_SystemContext _context;
        private readonly IAccountRepo _accountRepo;
        public LoginController(Library_Management_SystemContext context, IAccountRepo accountRepo)
        {
            _context = context;
            _accountRepo = accountRepo;
        }

        public IActionResult Index(string userName, string userPassword)
        {
            if (userName != null || userPassword != null)
            {
                HttpContext.Session.SetString("username", userName);
                var user = _accountRepo.getUserByName(userName);
                if (user == null)
                {
                    ViewBag.Message = "Invalid Credentials! Please Try Again";
                }
                else if (userName.Equals("admin") && userPassword.Equals("admin"))
                {
                    return RedirectToAction("Index","Admin");
                }
                else if (userName.Equals(user.UserName) && userPassword.Equals(user.UserPassword))
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ViewBag.Message = "Invalid Credentials! Please Try Again";
                }
            }
            return View();
        }
    }
}
