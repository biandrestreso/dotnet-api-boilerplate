using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAPI.Entities.DTO
{
    public class UserCreationDto
    {
        [Required(ErrorMessage = "First Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string? Surname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; } 
    }
}
