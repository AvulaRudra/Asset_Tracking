using Microsoft.EntityFrameworkCore;
using dotnet_backend.Data;
using dotnet_backend.Entities;
using dotnet_backend.Repositories;

namespace dotnet_backend.Repositories;

public class LocalUserRepository : ILocalUserRepository
{
    private readonly AppDbContext _db;

    public LocalUserRepository(AppDbContext db)
    {
        _db = db;
    }

    public LocalUser? GetById(string userId)
    {
        return _db.LocalUsers.Find(userId);
    }

    public LocalUser? GetByUsername(string username)
    {
        return _db.LocalUsers.FirstOrDefault(u => u.Username == username);
    }

    public LocalUser? GetByEmail(string email)
    {
        return _db.LocalUsers.FirstOrDefault(u => u.Email == email);
    }

    public LocalUser Create(LocalUser user)
    {
        _db.LocalUsers.Add(user);
        _db.SaveChanges();
        return user;
    }
}

