namespace CheaperThanAi.Shared.Requests;

using System.ComponentModel.DataAnnotations;

public sealed class SupportRequest
{
    // legacy field; not used by the client form but kept for compatibility
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your name.")]
    [StringLength(200, ErrorMessage = "Name is too long.")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Please enter your name.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please describe the issue.")]
    [StringLength(2000, ErrorMessage = "Message is too long.")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Please describe the issue.")]
    public string Message { get; set; } = string.Empty;
}
