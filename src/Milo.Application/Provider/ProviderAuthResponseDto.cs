namespace Milo.Application.Provider;

public record ProviderAuthResponseDto(
    Guid ProviderId,
    string Name,
    string Nit,
    string Email,
    string Role,
    string Token,
    DateTime ExpiresAt);