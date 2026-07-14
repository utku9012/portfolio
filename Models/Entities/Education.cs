using System.ComponentModel.DataAnnotations;

namespace portfolio.Models.Entities;

public class Education : IEntity
{
    public int Id { get; set; }

    public string? School { get; set; }

    public string? SchoolTr { get; set; }

    public string? Degree { get; set; }

    public string? DegreeTr { get; set; }

    [Display(Name = "GPA")]
    public string? Grade { get; set; }

    [Display(Name = "Graduation rank")]
    public string? GraduationRank { get; set; }

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
