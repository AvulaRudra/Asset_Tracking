using Microsoft.EntityFrameworkCore;
using dotnet_backend.Data;
using dotnet_backend.Entities;
using dotnet_backend.Repositories;
using dotnet_backend.DTOs;

namespace dotnet_backend.Repositories;

public class AssetTrackingRepository : IAssetTrackingRepository
{
    private readonly AppDbContext _context;

    public AssetTrackingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AssetTracking>> GetAllAsync()
    {
        return await _context.AssetTrackings.Include(t => t.Asset).ToListAsync();
    }

    public async Task<IEnumerable<AssetTracking>> GetByIdAsync(int assetId)
    {
        return await _context.AssetTrackings.Where(t => t.AssetId == assetId).Include(t => t.Asset).OrderByDescending(t => t.Timestamp).ToListAsync();
    }

    public async Task<IEnumerable<AssetTracking>> GetByAssetIdAsync(int assetId)
    {
        return await _context.AssetTrackings.Where(t => t.AssetId == assetId).Include(t => t.Asset).ToListAsync();
    }

    public async Task<AssetTracking> CreateAsync(CreateAssetTrackingDto trackingDto)
    {
        var tracking = new AssetTracking 
        { 
            Timestamp = DateTime.UtcNow,
            Latitude = trackingDto.Latitude,
            Longitude = trackingDto.Longitude,
            AssetId = trackingDto.AssetId
        };
        _context.AssetTrackings.Add(tracking);
        await _context.SaveChangesAsync();
        return tracking;
    }

    public async Task<AssetTracking?> UpdateAsync(int assetId, AssetTrackingDto trackingDto)
    {
        var tracking = await _context.AssetTrackings.Where(t => t.AssetId == assetId).OrderByDescending(t => t.Timestamp).FirstOrDefaultAsync();
        if (tracking == null) return null;

        tracking.Timestamp = DateTime.UtcNow;
        tracking.Latitude = trackingDto.Latitude;
        tracking.Longitude = trackingDto.Longitude;
        await _context.SaveChangesAsync();
        return tracking;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tracking = await _context.AssetTrackings.FindAsync(id);
        if (tracking == null) return false;

        _context.AssetTrackings.Remove(tracking);
        await _context.SaveChangesAsync();
        return true;
    }
}

