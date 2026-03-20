using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using dotnet_backend.Services;
using dotnet_backend.DTOs;
using dotnet_backend.Entities;
using static dotnet_backend.Entities.UserRoles;

namespace dotnet_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

    public AssetsController(IAssetService assetService)
    {
        _assetService = assetService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssets()
    {
        var assets = await _assetService.GetAllDtoAsync();
        return Ok(assets);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<AssetDto>> GetAsset(int id)
    {
        var asset = await _assetService.GetByIdDtoAsync(id);
        if (asset == null) return NotFound();
        return Ok(asset);
    }

    [HttpPost]
    [Authorize("Admin")]  // Admin CRUD
    public async Task<ActionResult<AssetDto>> CreateAsset(CreateAssetDto assetDto)
    {
        var asset = await _assetService.CreateAsync(assetDto);
        var assetDtoResult = new AssetDto
        {
            Id = asset.ID,
            Type = asset.Type,
            Trackings = new List<AssetTrackingDto>()
        };
        return CreatedAtAction(nameof(GetAsset), new { id = asset.ID }, assetDtoResult);
    }

    [HttpPut("{id}")]
    [Authorize("Admin")]
    public async Task<ActionResult<AssetDto>> UpdateAsset(int id, AssetDto assetDto)
    {
        var updated = await _assetService.UpdateAsync(id, assetDto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteAsset(int id)
    {
        var deleted = await _assetService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

