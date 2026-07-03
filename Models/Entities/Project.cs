using System.ComponentModel.DataAnnotations;

namespace portfolio.Models.Entities;

public class Project : IEntity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? NameTr { get; set; }

    public string? Summary { get; set; }

    public string? SummaryTr { get; set; }

    [Display(Name = "Display order")]
    public int DisplayOrder { get; set; }

    public string? Technologies { get; set; }

    public string? TechnologiesTr { get; set; }

    [Url]
    public string? Url { get; set; }

    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }
}
