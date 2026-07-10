namespace Milo.Application.Provider.Queries.GetProviders;

public record ProviderDto(
    Guid Id,
    string Name,
    string Nit,
    string PageUrl,
    string Email,
    string Role,
    bool IsActive);