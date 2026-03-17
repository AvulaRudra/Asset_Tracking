using dotnet_backend.Entities;

namespace dotnet_backend.Repositories;

public interface IUserRepository
{
    User? GetByProviderId(string provider, string userId);
    User Upsert(string provider, string userId, string? email);
}

