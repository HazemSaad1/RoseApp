using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Application.Contracts.IServices;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);
}