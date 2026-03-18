using System.Collections.Generic;
using dotnet_backend.DTOs;

namespace dotnet_backend.DTOs;

public class AssetDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public List<AssetTrackingDto> Trackings { get; set; } = new List<AssetTrackingDto>();
}
