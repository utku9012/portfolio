using portfolio.Models.Entities;
using portfolio.Models.ViewModels;
using portfolio.Repositories;

namespace portfolio.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IRepository<Profile> _profiles;
    private readonly IRepository<Experience> _experiences;
    private readonly IRepository<Project> _projects;
    private readonly IRepository<Education> _educations;
    private readonly IRepository<Skill> _skills;
    private readonly IRepository<Certification> _certifications;

    public PortfolioService(
        IRepository<Profile> profiles,
        IRepository<Experience> experiences,
        IRepository<Project> projects,
        IRepository<Education> educations,
        IRepository<Skill> skills,
        IRepository<Certification> certifications)
    {
        _profiles = profiles;
        _experiences = experiences;
        _projects = projects;
        _educations = educations;
        _skills = skills;
        _certifications = certifications;
    }

    public async Task<PortfolioViewModel> GetPortfolioAsync()
    {
        var profiles = await _profiles.GetAllAsync();
        var projects = await _projects.GetAllAsync();

        return new PortfolioViewModel
        {
            Profile = profiles.FirstOrDefault() ?? new Profile(),
            Experiences = await _experiences.GetAllAsync(),
            Projects = projects.OrderBy(project => project.DisplayOrder).ThenBy(project => project.Id).ToList(),
            Educations = await _educations.GetAllAsync(),
            Skills = await _skills.GetAllAsync(),
            Certifications = await _certifications.GetAllAsync()
        };
    }
}
