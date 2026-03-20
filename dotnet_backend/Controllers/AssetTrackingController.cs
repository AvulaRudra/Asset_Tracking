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
public class AssetTrackingController : ControllerBase
{
    private readonly IAssetTrackingService _trackingService;

    public AssetTrackingController(IAssetTrackingService trackingService)
    {
        _trackingService = trackingService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AssetTrackingDto>>> GetTrackings()
    {
        var trackings = await _trackingService.GetAllDtoAsync();
        return Ok(trackings);
    }

    [HttpGet("asset/{assetId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AssetTrackingDto>>> GetByAssetId(int assetId)
    {
        var trackings = await _trackingService.GetByAssetIdDtoAsync(assetId);
        return Ok(trackings);
    }

    [HttpPost("asset/{assetId}")]
    [Authorize("Admin")]  // Admin create tracking for asset
    public async Task<ActionResult<AssetTrackingDto>> CreateTracking(int assetId, CreateAssetTrackingDto trackingDto)
    {
        trackingDto.AssetId = assetId;  // Set from route
        var tracking = await _trackingService.CreateAsync(trackingDto);
        var trackingDtoResult = new AssetTrackingDto
        {
            Timestamp = tracking.Timestamp,
            Latitude = tracking.Latitude,
            Longitude = tracking.Longitude,
            AssetId = tracking.AssetId
        };
        return CreatedAtAction(nameof(GetByAssetId), new { assetId }, trackingDtoResult);
    }

    [HttpPut("asset/{assetId}")]
    [Authorize("Admin")]
    public async Task<ActionResult<AssetTrackingDto>> UpdateTracking(int assetId, AssetTrackingDto trackingDto)
    {
        var updated = await _trackingService.UpdateAsync(assetId, trackingDto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteTracking(int id)
    {
        var deleted = await _trackingService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
