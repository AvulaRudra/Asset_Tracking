using dotnet_backend.Entities;
using dotnet_backend.DTOs;

namespace dotnet_backend.Repositories;

public interface IAssetRepository
{
    Task<IEnumerable<Asset>> GetAllAsync();
    Task<Asset?> GetByIdAsync(int id);
    Task<Asset> CreateAsync(CreateAssetDto assetDto);
    Task<Asset?> UpdateAsync(int id, AssetDto assetDto);
    Task<bool> DeleteAsync(int id);
}

