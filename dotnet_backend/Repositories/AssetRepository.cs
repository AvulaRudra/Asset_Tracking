using Microsoft.EntityFrameworkCore;
using dotnet_backend.Data;
using dotnet_backend.Entities;
using dotnet_backend.Repositories;
using dotnet_backend.DTOs;

namespace dotnet_backend.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly AppDbContext _context;

    public AssetRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Asset>> GetAllAsync()
    {
        return await _context.Assets.Include(a => a.Trackings).ToListAsync();
    }

    public async Task<Asset?> GetByIdAsync(int id)
    {
        return await _context.Assets.Include(a => a.Trackings).FirstOrDefaultAsync(a => a.ID == id);
    }

    public async Task<Asset> CreateAsync(CreateAssetDto assetDto)
    {
        var asset = new Asset 
        { 
            Type = assetDto.Type 
        };
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return asset;
    }

    public async Task<Asset?> UpdateAsync(int id, AssetDto assetDto)
    {
        var asset = await _context.Assets.FindAsync(id);
        if (asset == null) return null;

        asset.Type = assetDto.Type;
        await _context.SaveChangesAsync();
        return asset;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var asset = await _context.Assets.FindAsync(id);
        if (asset == null) return false;

        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
        return true;
    }
}

