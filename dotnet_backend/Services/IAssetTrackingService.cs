using dotnet_backend.DTOs;
using dotnet_backend.Entities;

namespace dotnet_backend.Services;

public interface IAssetTrackingService
{
    Task<IEnumerable<AssetTracking>> GetAllAsync();
    Task<IEnumerable<AssetTracking?>> GetByIdAsync(int assetId);
    Task<AssetTracking> CreateAsync(CreateAssetTrackingDto trackingDto);
    Task<AssetTrackingDto?> UpdateAsync(int id, AssetTrackingDto trackingDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<AssetTrackingDto>> GetByAssetIdDtoAsync(int assetId);
    Task<IEnumerable<AssetTrackingDto>> GetAllDtoAsync();
}
