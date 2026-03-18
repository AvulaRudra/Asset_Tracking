using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using dotnet_backend.Repositories;
using dotnet_backend.DTOs;
using dotnet_backend.Entities;
using static dotnet_backend.Entities.UserRoles;

namespace dotnet_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssetsController : ControllerBase
{
    private readonly IAssetRepository _assetRepository;

    public AssetsController(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;
    }

    [HttpGet]
    [Authorize("Viewer")]  // Viewer can view
    public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
    {
        var assets = await _assetRepository.GetAllAsync();
        return Ok(assets);
    }

    [HttpGet("{id}")]
    [Authorize("Viewer")]
    public async Task<ActionResult<Asset>> GetAsset(int id)
    {
        var asset = await _assetRepository.GetByIdAsync(id);
        if (asset == null) return NotFound();
        return Ok(asset);
    }

    [HttpPost]
    [Authorize("Admin")]  // Admin CRUD
    public async Task<ActionResult<Asset>> CreateAsset(CreateAssetDto assetDto)
    {
        var asset = await _assetRepository.CreateAsync(assetDto);
        return CreatedAtAction(nameof(GetAsset), new { id = asset.ID }, asset);
    }

    [HttpPut("{id}")]
    [Authorize("Admin")]
    public async Task<ActionResult<Asset>> UpdateAsset(int id, AssetDto assetDto)
    {
        var updated = await _assetRepository.UpdateAsync(id, assetDto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteAsset(int id)
    {
        var deleted = await _assetRepository.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

