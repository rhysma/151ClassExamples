using System.ComponentModel.DataAnnotations;

namespace MegaTravelAPI.Models
{
    public class LoginModel
    {
        [Required]
        public string? Username { get; set; } = null;
        [Required]
        public string? Password { get; set; } = null;
        [Required]
        public string? UserType { get; set; } = null;
    }
}
