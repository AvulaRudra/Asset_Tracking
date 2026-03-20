using dotnet_backend.Entities;
using System.Threading.Tasks;

namespace dotnet_backend.Repositories;

public interface IUserRepository
{
    User? GetByProviderId(string provider, string userId);
    Task<User?> GetByProviderIdOrEmailAsync(string providerIdOrEmail);
    User Upsert(string provider, string userId, string? email);
    Task UpdateRoleAsync(string provider, string id, string newRole);
}

