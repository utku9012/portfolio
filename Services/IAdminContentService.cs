using portfolio.Models.Entities;
using portfolio.Models.ViewModels;

namespace portfolio.Services;

public interface IAdminContentService
{
    Task<PortfolioViewModel> GetDashboardAsync();
    Task SaveProfileAsync(Profile profile);
    Task AddExperienceAsync(Experience experience);
    Task UpdateExperienceAsync(Experience experience);
    Task DeleteExperienceAsync(int id);
    Task AddProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(int id);
    Task ReorderProjectsAsync(IReadOnlyList<int> projectIds);
    Task AddEducationAsync(Education education);
    Task UpdateEducationAsync(Education education);
    Task DeleteEducationAsync(int id);
    Task AddSkillAsync(Skill skill);
    Task UpdateSkillAsync(Skill skill);
    Task DeleteSkillAsync(int id);
    Task AddCertificationAsync(Certification certification);
    Task UpdateCertificationAsync(Certification certification);
    Task DeleteCertificationAsync(int id);
}
