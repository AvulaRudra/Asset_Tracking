using dotnet_backend.Data;
using dotnet_backend.DTOs;
using dotnet_backend.Entities;
using dotnet_backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace dotnet_backend.Services;

public class AssetTrackingService : IAssetTrackingService
{
    private readonly IAssetTrackingRepository _trackingRepository;

    public AssetTrackingService(IAssetTrackingRepository trackingRepository)
    {
        _trackingRepository = trackingRepository;
    }

    public async Task<IEnumerable<AssetTracking>> GetAllAsync()
    {
        return await _trackingRepository.GetAllAsync();
    }

    public async Task<IEnumerable<AssetTracking?>> GetByIdAsync(int assetId)
    {
        return await _trackingRepository.GetByIdAsync(assetId);
    }

    public async Task<IEnumerable<AssetTracking>> GetByAssetIdAsync(int assetId)
    {
        return await _trackingRepository.GetByAssetIdAsync(assetId);
    }

    public async Task<AssetTracking> CreateAsync(CreateAssetTrackingDto trackingDto)
    {
        return await _trackingRepository.CreateAsync(trackingDto);
    }

    public async Task<AssetTrackingDto?> UpdateAsync(int assetId, AssetTrackingDto trackingDto)
    {
        var tracking = await _trackingRepository.UpdateAsync(assetId, trackingDto);
        if (tracking == null) return null;
        return new AssetTrackingDto
        {
            Timestamp = tracking.Timestamp,
            Latitude = tracking.Latitude,
            Longitude = tracking.Longitude,
            AssetId = tracking.AssetId
        };
    }

    public async Task<IEnumerable<AssetTrackingDto>> GetAllDtoAsync()
    {
        var trackings = await _trackingRepository.GetAllAsync();
        return trackings.Select(t => new AssetTrackingDto
        {
            Timestamp = t.Timestamp,
            Latitude = t.Latitude,
            Longitude = t.Longitude,
            AssetId = t.AssetId
        });
    }

    public async Task<IEnumerable<AssetTrackingDto>> GetByAssetIdDtoAsync(int assetId)
    {
        var trackings = await _trackingRepository.GetByAssetIdAsync(assetId);
        return trackings.Select(t => new AssetTrackingDto
        {
            Timestamp = t.Timestamp,
            Latitude = t.Latitude,
            Longitude = t.Longitude,
            AssetId = t.AssetId
        });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _trackingRepository.DeleteAsync(id);
    }
}
