using portfolio.Models.Entities;

namespace portfolio.Models.ViewModels;

public class PortfolioViewModel
{
    public Profile Profile { get; set; } = new();
    public IReadOnlyList<Experience> Experiences { get; set; } = [];
    public IReadOnlyList<Project> Projects { get; set; } = [];
    public IReadOnlyList<Education> Educations { get; set; } = [];
    public IReadOnlyList<Skill> Skills { get; set; } = [];
    public IReadOnlyList<Certification> Certifications { get; set; } = [];
}
