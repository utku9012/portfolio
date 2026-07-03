using portfolio.Models.Entities;
using portfolio.Models.ViewModels;
using portfolio.Repositories;

namespace portfolio.Services;

public class AdminContentService : IAdminContentService
{
    private readonly IPortfolioService _portfolioService;
    private readonly IRepository<Profile> _profiles;
    private readonly IRepository<Experience> _experiences;
    private readonly IRepository<Project> _projects;
    private readonly IRepository<Education> _educations;
    private readonly IRepository<Skill> _skills;
    private readonly IRepository<Certification> _certifications;

    public AdminContentService(
        IPortfolioService portfolioService,
        IRepository<Profile> profiles,
        IRepository<Experience> experiences,
        IRepository<Project> projects,
        IRepository<Education> educations,
        IRepository<Skill> skills,
        IRepository<Certification> certifications)
    {
        _portfolioService = portfolioService;
        _profiles = profiles;
        _experiences = experiences;
        _projects = projects;
        _educations = educations;
        _skills = skills;
        _certifications = certifications;
    }

    public Task<PortfolioViewModel> GetDashboardAsync()
    {
        return _portfolioService.GetPortfolioAsync();
    }

    public async Task SaveProfileAsync(Profile profile)
    {
        var profiles = await _profiles.GetAllAsync();
        var existingProfile = profiles.FirstOrDefault();

        if (existingProfile is null)
        {
            await _profiles.AddAsync(profile);
        }
        else
        {
            profile.Id = existingProfile.Id;
            _profiles.Update(profile);
        }

        await _profiles.SaveChangesAsync();
    }

    public async Task AddExperienceAsync(Experience experience)
    {
        await _experiences.AddAsync(experience);
        await _experiences.SaveChangesAsync();
    }

    public async Task DeleteExperienceAsync(int id)
    {
        await _experiences.DeleteAsync(id);
        await _experiences.SaveChangesAsync();
    }

    public async Task UpdateExperienceAsync(Experience experience)
    {
        _experiences.Update(experience);
        await _experiences.SaveChangesAsync();
    }

    public async Task AddProjectAsync(Project project)
    {
        if (project.DisplayOrder <= 0)
        {
            var projects = await _projects.GetAllAsync();
            project.DisplayOrder = projects.Count == 0
                ? 1
                : projects.Max(existingProject => existingProject.DisplayOrder) + 1;
        }

        await _projects.AddAsync(project);
        await _projects.SaveChangesAsync();
    }

    public async Task DeleteProjectAsync(int id)
    {
        await _projects.DeleteAsync(id);
        await _projects.SaveChangesAsync();
    }

    public async Task UpdateProjectAsync(Project project)
    {
        _projects.Update(project);
        await _projects.SaveChangesAsync();
    }

    public async Task ReorderProjectsAsync(IReadOnlyList<int> projectIds)
    {
        var projects = await _projects.GetAllAsync();
        var projectsById = projects.ToDictionary(project => project.Id);

        for (var index = 0; index < projectIds.Count; index++)
        {
            if (projectsById.TryGetValue(projectIds[index], out var project))
            {
                project.DisplayOrder = index + 1;
                _projects.Update(project);
            }
        }

        await _projects.SaveChangesAsync();
    }

    public async Task AddEducationAsync(Education education)
    {
        await _educations.AddAsync(education);
        await _educations.SaveChangesAsync();
    }

    public async Task DeleteEducationAsync(int id)
    {
        await _educations.DeleteAsync(id);
        await _educations.SaveChangesAsync();
    }

    public async Task UpdateEducationAsync(Education education)
    {
        _educations.Update(education);
        await _educations.SaveChangesAsync();
    }

    public async Task AddSkillAsync(Skill skill)
    {
        await _skills.AddAsync(skill);
        await _skills.SaveChangesAsync();
    }

    public async Task DeleteSkillAsync(int id)
    {
        await _skills.DeleteAsync(id);
        await _skills.SaveChangesAsync();
    }

    public async Task UpdateSkillAsync(Skill skill)
    {
        _skills.Update(skill);
        await _skills.SaveChangesAsync();
    }

    public async Task AddCertificationAsync(Certification certification)
    {
        await _certifications.AddAsync(certification);
        await _certifications.SaveChangesAsync();
    }

    public async Task DeleteCertificationAsync(int id)
    {
        await _certifications.DeleteAsync(id);
        await _certifications.SaveChangesAsync();
    }

    public async Task UpdateCertificationAsync(Certification certification)
    {
        _certifications.Update(certification);
        await _certifications.SaveChangesAsync();
    }
}
