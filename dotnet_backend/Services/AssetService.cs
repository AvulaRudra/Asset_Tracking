using dotnet_backend.Data;
using dotnet_backend.DTOs;
using dotnet_backend.Entities;
using dotnet_backend.Repositories;
using dotnet_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace dotnet_backend.Services;

public class AssetService : IAssetService
{
    private readonly IAssetRepository _assetRepository;

    public AssetService(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<IEnumerable<Asset>> GetAllAsync()
    {
        return await _assetRepository.GetAllAsync();
    }

    public async Task<Asset?> GetByIdAsync(int id)
    {
        return await _assetRepository.GetByIdAsync(id);
    }

    public async Task<Asset> CreateAsync(CreateAssetDto assetDto)
    {
        return await _assetRepository.CreateAsync(assetDto);
    }

    public async Task<Asset?> UpdateAsync(int id, AssetDto assetDto)
    {
        return await _assetRepository.UpdateAsync(id, assetDto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _assetRepository.DeleteAsync(id);
    }
}
