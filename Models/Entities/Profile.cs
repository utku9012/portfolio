using System.ComponentModel.DataAnnotations;

namespace portfolio.Models.Entities;

public class Profile : IEntity
{
    public int Id { get; set; }

    [Required, Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    public string? Title { get; set; }

    [Display(Name = "Title TR")]
    public string? TitleTr { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Phone { get; set; }
    public string? Location { get; set; }

    public string? Summary { get; set; }

    [Display(Name = "Summary TR")]
    public string? SummaryTr { get; set; }

    [Url, Display(Name = "LinkedIn URL")]
    public string? LinkedInUrl { get; set; }

    [Url, Display(Name = "GitHub URL")]
    public string? GitHubUrl { get; set; }

    [Display(Name = "Photo URL")]
    public string? PhotoUrl { get; set; }

    [Display(Name = "CV URL")]
    public string? CvUrl { get; set; }
}
