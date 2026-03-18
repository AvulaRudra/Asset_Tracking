using dotnet_backend.Data;
using dotnet_backend.DTOs;
using dotnet_backend.Entities;
using dotnet_backend.Repositories;
using dotnet_backend.Services;
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

    public async Task<AssetTracking?> UpdateAsync(int assetId, AssetTrackingDto trackingDto)
    {
        return await _trackingRepository.UpdateAsync(assetId, trackingDto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _trackingRepository.DeleteAsync(id);
    }
}
