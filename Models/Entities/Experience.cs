using System.ComponentModel.DataAnnotations;

namespace portfolio.Models.Entities;

public class Experience : IEntity
{
    public int Id { get; set; }

    public string? Role { get; set; } 

    public string? RoleTr { get; set; }

    [Required]
    public string Company { get; set; } = string.Empty;

    public string? Location { get; set; }

    public string? LocationTr { get; set; }

    [Display(Name = "Start date")]
    public string? StartDate { get; set; }

    [Display(Name = "Start date TR")]
    public string? StartDateTr { get; set; }

    [Display(Name = "End date")]
    public string? EndDate { get; set; }

    [Display(Name = "End date TR")]
    public string? EndDateTr { get; set; }

    public string? Description { get; set; }

    public string? DescriptionTr { get; set; }
}
