namespace dotnet_backend.DTOs;

public class CreateAssetTrackingDto
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public int AssetId { get; set; }
}
