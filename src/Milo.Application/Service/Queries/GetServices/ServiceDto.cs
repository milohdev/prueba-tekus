namespace Milo.Application.Service.Queries.GetServices;

public record ServiceDto(
    Guid Id,
    string Name,
    decimal CostPerHour,
    Guid ProviderId,
    bool IsActive);