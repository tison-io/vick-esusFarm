namespace eSusFarm.Api.Models;

public class UssdResponse
{
    public string Message { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public bool SessionActive { get; set; }
}
