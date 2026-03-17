using dotnet_backend.Entities;

namespace dotnet_backend.Repositories;

public interface ILocalUserRepository
{
    LocalUser? GetById(string userId);
    LocalUser? GetByUsername(string username);
    LocalUser? GetByEmail(string email);
    LocalUser Create(LocalUser user);
}

