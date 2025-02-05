using System.Threading.Tasks;
using LibraryManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagement.Controllers
{
    public class UserController(DataContext context) : Controller
    {
        private readonly DataContext _context = context;

        private bool IsAdmin()  // Admin kontrolü
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "").ToLower();
            }
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");  // Giriş yapılmamışsa logine yönlendir
            }

            if (!IsAdmin())  // Kullanıcı admin değilse hata mesajı dönderip anasayfaya yönlendirir.
            {
                TempData["ErrorMessage"] = "Yetkisiz giriş! Bu sayfaya erişim izniniz yok.";
                return RedirectToAction("Index", "Home"); // Anasayfaya yönlendirir
            }

            return View(await _context.Users.ToListAsync());
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)  // Kullanıcının var olup olmadığının kontrolü
            {
                ViewBag.ErrorMessage = "Bu e-posta adresi zaten kayıtlı!";
                return View(model);
            }

            model.Password = HashPassword(model.Password);
            _context.Users.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult CreateAdmin()
        {
            if (!IsAdmin()) return Unauthorized();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(User model)  // Admin oluşturma işlemleri
        {
            if (!IsAdmin()) return Unauthorized();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Bu e-posta adresi zaten kayıtlı!";
                return View(model);
            }

            model.Role = "Admin";
            model.Password = HashPassword(model.Password);
            _context.Users.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);

            if (user == null)
            {
                ViewBag.ErrorMessage = "E-posta veya şifre hatalı!";
                return View();
            }

            HttpContext.Session.SetString("UserEmail", user.Email); // Session'a user'ın email 
            HttpContext.Session.SetString("UserRole", user.Role);   // ve rol bilgilerini atıyor

            if (user.Role == "Admin")  // Giriş yapan admin ise user sayfasına yönlendiriyor
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                return RedirectToAction("Index", "Book");  // admin değilse kitap sayfasına yönlendiriyor
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  // Çıkış yapılınca session'da olan bilgileri temizliyor
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Profile() // Profil görüntüleme işlemleri
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login");  // Giriş yapılmamışsa logine yönlendiriyor
            }

            string userEmail = HttpContext.Session.GetString("UserEmail");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound();
            }

            var borrowedBooks = await _context.Books  // Mevcut kullanıcının ödünç aldığı kitapları getiriyor
                .Where(b => b.BorrowedBy == user.Email)
                .ToListAsync();

            ViewBag.BorrowedBooks = borrowedBooks;
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin()) return Unauthorized();
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User model)
        {
            if (!IsAdmin()) return Unauthorized();

            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.Password = HashPassword(model.Password);
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Users.Any(u => u.Id == model.Id))
                    {
                        return NotFound();
                    }
                    else { throw; }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin()) return Unauthorized();

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            if (!IsAdmin()) return Unauthorized();
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Password != HashPassword(oldPassword))
            {
                ViewBag.ErrorMessage = "Mevcut şifre yanlış!";
                return View();
            }

            user.Password = HashPassword(newPassword);
            await _context.SaveChangesAsync();

            ViewBag.SuccessMessage = "Şifreniz başarıyla değiştirildi.";
            return View();
        }
    }
}
