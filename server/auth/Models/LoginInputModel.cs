using System.ComponentModel.DataAnnotations;

namespace net8auth.auth.Models
{
    public record LoginInputModel
    {
        [Required]
        public string Username { get; init; } = string.Empty;
        [Required]
        public string Password { get; init; } = string.Empty;
        public bool RememberLogin { get; init; }
        public string ReturnUrl { get; init; } = string.Empty;
    }
}