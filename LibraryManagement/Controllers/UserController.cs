using System.Threading.Tasks;
using LibraryManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Added for Identity
using Microsoft.AspNetCore.Authorization; // Added for Authorization
using LibraryManagement.Models; // Added for ChangePasswordViewModel
// using System.Security.Cryptography; // No longer needed for HashPassword
// using System.Text; // No longer needed for HashPassword

namespace LibraryManagement.Controllers
{
    [Authorize] // Require authentication for user-related actions
    public class UserController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        // SignInManager might be needed if UserController handles login/logout, but we moved it to AccountController

        public UserController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Removed IsAdmin() and HashPassword() as Identity handles this.

        [Authorize(Roles = "Admin")] // Only Admins can see the user list
        public async Task<IActionResult> Index()
        {
            // Listing ApplicationUsers now
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // Register, Login, Logout actions are moved to AccountController.
        // CreateAdmin might be re-implemented using UserManager if needed, or managed via roles.

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> CreateAdmin() // Placeholder - actual admin creation should be robust
        {
            // This is a simplified view. Proper admin creation might involve selecting a user and adding them to Admin role.
            // Or a dedicated registration path for admins.
            // For now, this action might not be directly used if registration creates standard users
            // and an existing admin elevates them.
            return View(); // Needs a CreateAdmin.cshtml view if kept
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(string email, string password, string fullName) // Simplified model
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName))
            {
                ModelState.AddModelError("", "All fields are required.");
                return View(); // Needs a CreateAdmin.cshtml view
            }

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "This email address is already registered.");
                return View();
            }

            var adminUser = new ApplicationUser { UserName = email, Email = email, FullName = fullName, MembershipDate = DateTime.UtcNow };
            var result = await _userManager.CreateAsync(adminUser, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin"); // Ensure "Admin" role exists
                TempData["SuccessMessage"] = $"Admin user {email} created successfully.";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(); // Needs a CreateAdmin.cshtml view
        }


        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // This case should ideally not happen if [Authorize] is effective
                return Challenge(); // Or RedirectToAction("Login", "Account");
            }

            // To get borrowed books, we now look for BorrowedByUserId
            var borrowedBooks = await _context.Books
                .Where(b => b.BorrowedByUserId == user.Id)
                .Include(b => b.Borrower) // Include borrower details if needed in the view
                .ToListAsync();

            ViewBag.BorrowedBooks = borrowedBooks;
            return View(user); // Pass ApplicationUser to the view
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id) // Id is string (ApplicationUser.Id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // Create a ViewModel for editing user details if necessary, to avoid overposting
            return View(user); // Pass ApplicationUser to an Edit.cshtml view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, ApplicationUser model) // model should be a ViewModel
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            // This is a simplified Edit. For a real scenario, use a ViewModel and map properties.
            // Password change should be a separate, secure process.
            // Role management should also be distinct.
            if (ModelState.IsValid) // Basic validation
            {
                var userToUpdate = await _userManager.FindByIdAsync(id);
                if (userToUpdate == null) return NotFound();

                userToUpdate.FullName = model.FullName; // Example: Update FullName
                userToUpdate.Email = model.Email;         // Example: Update Email (ensure unique if changed)
                userToUpdate.UserName = model.Email;      // Keep UserName in sync with Email

                var result = await _userManager.UpdateAsync(userToUpdate);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User updated successfully.";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model); // Return to Edit.cshtml view with the model
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id) // Id is string
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user); // Pass ApplicationUser to a Delete.cshtml view
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "User deleted successfully.";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description); // Show errors on a view or redirect with TempData
                }
                // If deletion failed, redirect to Index or show errors
                TempData["ErrorMessage"] = "Error deleting user.";
                return RedirectToAction("Index");
            }
            return NotFound();
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            // This view would typically show fields for OldPassword, NewPassword, ConfirmPassword
            return View(); // Needs a ChangePassword.cshtml view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model) // Needs a ChangePasswordViewModel
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge(); // Should not happen if authorized
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // Optionally sign in the user again if password change invalidates the cookie
            // await _signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "Your password has been changed successfully.";
            return RedirectToAction("Profile"); // Or a dedicated success page
        }
    }
}

