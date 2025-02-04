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

        public bool IsAvailable { get; set; } = true;

        public int? RentedByUserId { get; set; }

        public User? RentedByUser { get; set; }
    }
}