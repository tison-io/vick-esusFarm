using System.ComponentModel.DataAnnotations;

namespace eSusFarm.Api.Models;

public class UssdRequest
{
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Session ID is required.")]
    public string SessionId { get; set; } = string.Empty;

    public string UserInput { get; set; } = string.Empty;
}
