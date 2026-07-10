using Milo.Domain.Entities;

namespace Milo.Application.Common.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
    (string Token, DateTime ExpiresAt) GenerateToken(Milo.Domain.Entities.Provider provider);
}
