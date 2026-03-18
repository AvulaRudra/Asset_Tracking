using dotnet_backend.DTOs;
using dotnet_backend.Entities;

namespace dotnet_backend.Services;

public interface IAssetService
{
    Task<IEnumerable<Asset>> GetAllAsync();
    Task<Asset?> GetByIdAsync(int id);
    Task<Asset> CreateAsync(CreateAssetDto assetDto);
    Task<Asset?> UpdateAsync(int id, AssetDto assetDto);
    Task<bool> DeleteAsync(int id);
}
