using eSusFarm.Api.Models;

namespace eSusFarm.Api.Services;

public interface IUssdRegistrationService
{
    UssdResponse ProcessRegistration(UssdRequest request);
}
