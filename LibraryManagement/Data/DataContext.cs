using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{

    public class DataContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        // The existing DbSet<User> is removed as ApplicationUser, managed by IdentityDbContext, replaces its functionality.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // IMPORTANT: Call base method first

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasOne(b => b.Borrower)
                      .WithMany() // No corresponding collection navigation property on ApplicationUser
                      .HasForeignKey(b => b.BorrowedByUserId)
                      .IsRequired(false) // A book is not always borrowed
                      .OnDelete(DeleteBehavior.SetNull); // If a user is deleted, set BorrowedByUserId to null
            });
        }
    }
}