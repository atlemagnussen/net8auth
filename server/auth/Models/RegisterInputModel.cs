using System.ComponentModel.DataAnnotations;

namespace net8auth.auth.Pages;

public record RegisterInputModel
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}