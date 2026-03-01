using System.Collections.Concurrent;
using eSusFarm.Api.Models;

namespace eSusFarm.Api.Services;

public class UssdRegistrationService : IUssdRegistrationService
{
    private const int MaxResponseLength = 160;

    private readonly ConcurrentDictionary<string, FarmerSession> _sessions = new();
    private readonly ILogger<UssdRegistrationService> _logger;

    public UssdRegistrationService(ILogger<UssdRegistrationService> logger)
    {
        _logger = logger;
    }

    public UssdResponse ProcessRegistration(UssdRequest request)
    {
        if (!_sessions.TryGetValue(request.SessionId, out var session))
        {
            return HandleNewSession(request);
        }

        return session.CurrentStep switch
        {
            SessionStep.AwaitingDistrict => HandleDistrictInput(request, session),
            SessionStep.AwaitingFarmSize => HandleFarmSizeInput(request, session),
            _ => BuildResponse(request.SessionId, "An error occurred. Please try again.", sessionActive: false)
        };
    }

    private UssdResponse HandleNewSession(UssdRequest request)
    {
        var session = new FarmerSession
        {
            PhoneNumber = request.PhoneNumber,
            CurrentStep = SessionStep.AwaitingDistrict
        };

        _sessions[request.SessionId] = session;

        _logger.LogInformation("New registration session started: {SessionId} for {PhoneNumber}",
            request.SessionId, request.PhoneNumber);

        return BuildResponse(request.SessionId,
            "Welcome to eSusFarm. Please enter your District:",
            sessionActive: true);
    }

    private UssdResponse HandleDistrictInput(UssdRequest request, FarmerSession session)
    {
        var district = request.UserInput.Trim();

        if (string.IsNullOrWhiteSpace(district))
        {
            return BuildResponse(request.SessionId,
                "District cannot be empty. Please enter your District:",
                sessionActive: true);
        }

        if (district.Length > 100)
        {
            return BuildResponse(request.SessionId,
                "District name is too long. Please enter a shorter name:",
                sessionActive: true);
        }

        session.District = district;
        session.CurrentStep = SessionStep.AwaitingFarmSize;

        _logger.LogInformation("Session {SessionId}: District set to {District}",
            request.SessionId, district);

        return BuildResponse(request.SessionId,
            "Please enter your Farm Size in hectares:",
            sessionActive: true);
    }

    private UssdResponse HandleFarmSizeInput(UssdRequest request, FarmerSession session)
    {
        var input = request.UserInput.Trim();

        if (!decimal.TryParse(input, out var farmSize) || farmSize <= 0)
        {
            return BuildResponse(request.SessionId,
                "Invalid farm size. Please enter a positive number in hectares:",
                sessionActive: true);
        }

        if (farmSize > 100_000)
        {
            return BuildResponse(request.SessionId,
                "Farm size seems too large. Please enter a valid size in hectares:",
                sessionActive: true);
        }

        session.FarmSizeHectares = farmSize;

        _logger.LogInformation(
            "Session {SessionId}: Registration complete. Phone={PhoneNumber}, District={District}, FarmSize={FarmSize}ha",
            request.SessionId, session.PhoneNumber, session.District, farmSize);

        _sessions.TryRemove(request.SessionId, out _);

        return BuildResponse(request.SessionId,
            "Registration complete. Thank you!",
            sessionActive: false);
    }

    private static UssdResponse BuildResponse(string sessionId, string message, bool sessionActive)
    {
        if (message.Length > MaxResponseLength)
        {
            message = message[..(MaxResponseLength - 3)] + "...";
        }

        return new UssdResponse
        {
            SessionId = sessionId,
            Message = message,
            SessionActive = sessionActive
        };
    }
}
