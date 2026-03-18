using dotnet_backend.Entities;
using dotnet_backend.DTOs;

namespace dotnet_backend.Repositories;

public interface IAssetTrackingRepository
{
    Task<IEnumerable<AssetTracking>> GetAllAsync();
    Task<IEnumerable<AssetTracking?>> GetByIdAsync(int assetId);
    Task<AssetTracking> CreateAsync(CreateAssetTrackingDto trackingDto);
    Task<AssetTracking?> UpdateAsync(int id, AssetTrackingDto trackingDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<AssetTracking>> GetByAssetIdAsync(int assetId);
}

