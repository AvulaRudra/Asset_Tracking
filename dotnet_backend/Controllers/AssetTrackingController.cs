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
    [Authorize("Viewer")]  // Viewer can view all
    public async Task<ActionResult<IEnumerable<dynamic>>> GetTrackings()
    {
        var trackings = await _trackingService.GetAllAsync();
        return Ok(trackings);
    }

    [HttpGet("asset/{assetId}")]
    [Authorize("Viewer")]
    public async Task<ActionResult<IEnumerable<dynamic>>> GetByAssetId(int assetId)
    {
        var trackings = await _trackingService.GetByAssetIdAsync(assetId);
        return Ok(trackings);
    }

    [HttpPost("asset/{assetId}")]
    [Authorize(UserRoles.Admin)]  // Admin create tracking for asset
    public async Task<ActionResult<AssetTracking>> CreateTracking(int assetId, CreateAssetTrackingDto trackingDto)
    {
        trackingDto.AssetId = assetId;  // Set from route
        var tracking = await _trackingService.CreateAsync(trackingDto);
        return CreatedAtAction(nameof(GetByAssetId), new { assetId }, tracking);
    }

    [HttpPut("asset/{assetId}")]
    [Authorize(UserRoles.Admin)]
    public async Task<ActionResult<AssetTracking>> UpdateTracking(int assetId, AssetTrackingDto trackingDto)
    {
        var updated = await _trackingService.UpdateAsync(assetId, trackingDto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize(UserRoles.Admin)]
    public async Task<IActionResult> DeleteTracking(int id)
    {
        var deleted = await _trackingService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

