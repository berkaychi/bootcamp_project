using System.Threading.Tasks;
using LibraryManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagement.Controllers
{
    public class BookController(DataContext context) : Controller
    {
        private readonly DataContext _context = context;

        private bool IsAdmin()  // Admin rolü kontrolü
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Users = await _context.Users.Where(u => u.Role == "User").ToListAsync();
            return View(await _context.Books.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return Unauthorized();
            ViewBag.Categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "Roman", Text = "Roman" },
                new SelectListItem { Value = "Bilim Kurgu", Text = "Bilim Kurgu" },
                new SelectListItem { Value = "Fantastik", Text = "Fantastik" },
                new SelectListItem { Value = "Tarih", Text = "Tarih" },
                new SelectListItem { Value = "Biyografi", Text = "Biyografi" },
                new SelectListItem { Value = "Kişisel Gelişim", Text = "Kişisel Gelişim" },
                new SelectListItem { Value = "Polisiye", Text = "Polisiye" }
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book model)
        {
            if (!IsAdmin()) return Unauthorized();  // Kullanıcı admin değilse sayfaya erişilemez.

            _context.Books.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin()) return Unauthorized();

            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new List<SelectListItem>  // kategori seçimi için hazır liste
            {
                new SelectListItem { Value = "Roman", Text = "Roman" },
                new SelectListItem { Value = "Bilim Kurgu", Text = "Bilim Kurgu" },
                new SelectListItem { Value = "Fantastik", Text = "Fantastik" },
                new SelectListItem { Value = "Tarih", Text = "Tarih" },
                new SelectListItem { Value = "Biyografi", Text = "Biyografi" },
                new SelectListItem { Value = "Kişisel Gelişim", Text = "Kişisel Gelişim" },
                new SelectListItem { Value = "Polisiye", Text = "Polisiye" }
            };

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book model)
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
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Books.Any(o => o.Id == model.Id))
                    {
                        return NotFound();
                    }
                    else { throw; }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin()) return Unauthorized();

            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            if (!IsAdmin()) return Unauthorized();

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Borrow(int id)  // Kitap ödünç alma işlemleri
        {
            if (IsAdmin()) return Unauthorized();  // Adminler kitap alamaz

            var book = await _context.Books.FindAsync(id);
            if (book == null || !string.IsNullOrEmpty(book.BorrowedBy))  // kitap yoksa veya başkasındaysa
            {
                return NotFound();
            }

            book.BorrowedBy = HttpContext.Session.GetString("UserEmail");
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Return(int id)  // kitabı geri verme işlemi
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            if (book.BorrowedBy == HttpContext.Session.GetString("UserEmail") || IsAdmin())  // kitabın sahibiyse veya
            {                                                                                //admin ise işlem yapabilir
                book.BorrowedBy = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AssignBook(int bookId, int userId)  // Adminlerin kullanıcılara kitap tanımlama işlemi
        {
            if (!IsAdmin()) return Unauthorized();

            var book = await _context.Books.FindAsync(bookId);
            var user = await _context.Users.FindAsync(userId);

            if (book == null || user == null || !string.IsNullOrEmpty(book.BorrowedBy))
            {
                return NotFound();
            }

            book.BorrowedBy = user.Email;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
