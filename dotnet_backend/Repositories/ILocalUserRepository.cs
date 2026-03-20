using dotnet_backend.Entities;
using System.Threading.Tasks;

namespace dotnet_backend.Repositories;

public interface ILocalUserRepository
{
    LocalUser? GetById(string userId);
    LocalUser? GetByUsername(string username);
    LocalUser? GetByEmail(string email);
    LocalUser Create(LocalUser user);
    Task<LocalUser?> GetByIdOrEmailAsync(string idOrEmail);
    Task UpdateRoleAsync(string id, string newRole);
}

