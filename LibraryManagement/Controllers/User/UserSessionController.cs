using LibraryManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers.User
{
    public class UserSessionController(DataContext context) : Controller
    {
        private readonly DataContext _context = context;

        public IActionResult Login()
        {
            return View();
        }
    }
}