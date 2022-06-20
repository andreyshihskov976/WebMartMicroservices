using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class LoginData
    {
        [Required]
        public string? UserLogin { get; set; }
        [Required]
        public string? UserPassword { get; set; }
    }
}