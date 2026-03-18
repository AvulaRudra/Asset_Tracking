using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_backend.Entities;

public class AssetTracking
{
    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public decimal Latitude { get; set; }
    
    [Required]
    public decimal Longitude { get; set; }

    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    public virtual Asset? Asset { get; set; }
}
