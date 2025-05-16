using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = default!;

        [DataType(DataType.Date)]
        [Display(Name = "Membership Date")]
        public DateTime MembershipDate { get; set; } = DateTime.UtcNow;
    }
}