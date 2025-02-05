using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Data
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ad Soyad")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "User";
    }
}