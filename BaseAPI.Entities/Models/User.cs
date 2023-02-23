using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseAPI.Entities.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public byte[]? Salt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = "user";

        // eg. one-to-many relationship
        //
        // [ForeignKey(nameof(Job))]
        // public Guid? JobId { get; set; }
        // public Job? Job { get; set; }

    }
}
