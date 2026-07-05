using System.ComponentModel.DataAnnotations;

namespace portfolio.Models.Entities;

public class UploadedAsset
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [MaxLength(120)]
    public string ContentType { get; set; } = string.Empty;

    public byte[] Data { get; set; } = [];

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
