using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLocks.Shared.Models
{
    public class UserCredential
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[A-Za-z0-9_\\+-]+(\\.[A-Za-z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? HashedPassword { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public string? Token { get; set; }
        [NotMapped] public string? Role { get; set; }
    }
}
