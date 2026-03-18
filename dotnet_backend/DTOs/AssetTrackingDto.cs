using System;

namespace dotnet_backend.DTOs;

public class AssetTrackingDto
{
    public DateTime Timestamp { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public int AssetId { get; set; }
}
