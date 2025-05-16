using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Data
{
    public class Book
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public string Author { get; set; } = default!;

        public int? PublishYear { get; set; }

        public string? Category { get; set; }

        public string? BorrowedByUserId { get; set; } // Foreign key to ApplicationUser
        public ApplicationUser? Borrower { get; set; } // Navigation property
    }
}