using LibraryManagement.Data;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{

    public class UserController(DataContext context) : Controller
    {

        private readonly DataContext _context = context;
    }
}