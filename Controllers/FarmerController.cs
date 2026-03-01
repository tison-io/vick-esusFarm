using eSusFarm.Api.Models;
using eSusFarm.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace eSusFarm.Api.Controllers;

[ApiController]
[Route("api/ussd")]
public class FarmerController : ControllerBase
{
    private readonly IUssdRegistrationService _registrationService;

    public FarmerController(IUssdRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UssdRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(new { errors });
        }

        var response = _registrationService.ProcessRegistration(request);
        return Ok(response);
    }
}
