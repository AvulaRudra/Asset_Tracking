using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace dotnet_backend.Entities;

public class Asset
{
    [Key]
    [Required]
    public int ID { get; set; }

    [Required]
    public string Type { get; set; }
    
    public virtual ICollection<AssetTracking> Trackings { get; set; } = new List<AssetTracking>();
}
