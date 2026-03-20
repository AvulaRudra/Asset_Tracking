using Microsoft.EntityFrameworkCore;
using dotnet_backend.Data;
using dotnet_backend.Entities;
using dotnet_backend.Repositories;

namespace dotnet_backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public User? GetByProviderId(string provider, string userId)
    {
        return _db.Users.FirstOrDefault(u => u.Provider == provider && u.Id == userId);
    }

    public async Task<User?> GetByProviderIdOrEmailAsync(string providerIdOrEmail)
    {
        if (providerIdOrEmail.Contains("@"))
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == providerIdOrEmail);
        }
        return await _db.Users.FirstOrDefaultAsync(u => u.Id == providerIdOrEmail);
    }

    public User Upsert(string provider, string userId, string? email)
    {
        var user = GetByProviderId(provider, userId);
        if (user == null)
        {
            user = new User { Id = userId, Provider = provider, Email = email };
            _db.Users.Add(user);
        }
        else
        {
            user.Email = email;
            _db.Users.Update(user);
        }
        _db.SaveChanges();
        return user;
    }

    public async Task UpdateRoleAsync(string provider, string id, string newRole)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Provider == provider && u.Id == id);
        if (user != null)
        {
            user.Role = newRole;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }
}

