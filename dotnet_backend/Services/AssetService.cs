using dotnet_backend.Data;
using dotnet_backend.DTOs;
using dotnet_backend.Entities;
using dotnet_backend.Repositories;
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

    public async Task<AssetDto?> GetByIdDtoAsync(int id)
    {
        var asset = await _assetRepository.GetByIdAsync(id);
        if (asset == null) return null;
        return new AssetDto
        {
            Id = asset.ID,
            Type = asset.Type,
            Trackings = asset.Trackings.Select(t => new AssetTrackingDto
            {
                Timestamp = t.Timestamp,
                Latitude = t.Latitude,
                Longitude = t.Longitude,
                AssetId = t.AssetId
            }).ToList()
        };
    }

    public async Task<Asset> CreateAsync(CreateAssetDto assetDto)
    {
        return await _assetRepository.CreateAsync(assetDto);
    }

    public async Task<IEnumerable<AssetDto>> GetAllDtoAsync()
    {
        var assets = await _assetRepository.GetAllAsync();
        return assets.Select(a => new AssetDto
        {
            Id = a.ID,
            Type = a.Type,
            Trackings = a.Trackings.Select(t => new AssetTrackingDto
            {
                Timestamp = t.Timestamp,
                Latitude = t.Latitude,
                Longitude = t.Longitude,
                AssetId = t.AssetId
            }).ToList()
        });
    }

    public async Task<AssetDto?> UpdateAsync(int id, AssetDto assetDto)
    {
        var asset = await _assetRepository.UpdateAsync(id, assetDto);
        if (asset == null) return null;
        return new AssetDto
        {
            Id = asset.ID,
            Type = asset.Type,
            Trackings = asset.Trackings.Select(t => new AssetTrackingDto
            {
                Timestamp = t.Timestamp,
                Latitude = t.Latitude,
                Longitude = t.Longitude,
                AssetId = t.AssetId
            }).ToList()
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _assetRepository.DeleteAsync(id);
    }
}
