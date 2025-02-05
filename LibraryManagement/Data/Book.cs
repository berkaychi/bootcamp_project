using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Data
{
    public class Book
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public int? PublishYear { get; set; }

        public string? Category { get; set; }

        public string? BorrowedBy { get; set; } // Ödünç alan kullanıcının e-posta bilgisi
    }
}