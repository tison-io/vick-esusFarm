namespace eSusFarm.Api.Models;

public class FarmerSession
{
    public string PhoneNumber { get; set; } = null!;
    public string? District { get; set; }
    public decimal? FarmSizeHectares { get; set; }
    public SessionStep CurrentStep { get; set; } = SessionStep.AwaitingDistrict;
}

public enum SessionStep
{
    AwaitingDistrict,
    AwaitingFarmSize
}
