using System.Threading.Tasks;
using LibraryManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // Keep for other potential uses if any, or remove if not needed.
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity; // Added for Identity
using Microsoft.AspNetCore.Authorization; // Added for Authorization

namespace LibraryManagement.Controllers
{
    [Authorize] // Require authentication for all actions in this controller by default
    public class BookController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous] // Allow anonymous access to the book list
        public async Task<IActionResult> Index()
        {
            // Fetching users for dropdown (if still needed for assign) should use ApplicationUser
            ViewBag.Users = await _userManager.Users.ToListAsync(); // Consider filtering by role if needed
            ViewBag.CurrentUserId = _userManager.GetUserId(User); // Pass current user's ID for view logic

            var books = await _context.Books.Include(b => b.Borrower).ToListAsync();

            return View(books);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Only Admins can create
        public IActionResult Create()
        {
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Book model)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            // Re-populate ViewBag.Categories if returning to view due to invalid ModelState
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
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Book model)
        {
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
            // Re-populate ViewBag.Categories if returning to view
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
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(int id)
        {
            if (User.IsInRole("Admin")) // Admins cannot borrow books directly through this action
            {
                TempData["ErrorMessage"] = "Admins cannot borrow books directly. Use Assign Book feature.";
                return RedirectToAction("Index");
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null || !string.IsNullOrEmpty(book.BorrowedByUserId))
            {
                TempData["ErrorMessage"] = "Book not available or already borrowed.";
                return RedirectToAction("Index");
            }

            var userId = _userManager.GetUserId(User);
            book.BorrowedByUserId = userId;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Book '{book.Title}' borrowed successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            // Allow return if the current user borrowed it OR if the current user is an Admin
            if (book.BorrowedByUserId == currentUserId || User.IsInRole("Admin"))
            {
                book.BorrowedByUserId = null;
                book.Borrower = null; // Clear navigation property
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Book '{book.Title}' returned successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "You are not authorized to return this book.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignBook(int bookId, string userId) // userId is string (ApplicationUser.Id)
        {
            var book = await _context.Books.FindAsync(bookId);
            var user = await _userManager.FindByIdAsync(userId);

            if (book == null || user == null)
            {
                TempData["ErrorMessage"] = "Book or User not found.";
                return RedirectToAction("Index");
            }
            if (!string.IsNullOrEmpty(book.BorrowedByUserId))
            {
                TempData["ErrorMessage"] = "Bu kitap zaten başka bir kullanıcıya ödünç verilmiş.";
                return RedirectToAction("Index");
            }


            book.BorrowedByUserId = user.Id;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Book '{book.Title}' assigned to {user.FullName} successfully.";
            return RedirectToAction("Index");
        }
    }
}
