using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using portfolio.Data;
using portfolio.Models.Entities;
using portfolio.Models.ViewModels;
using portfolio.Services;

namespace portfolio.Controllers;

public class AdminController : Controller
{
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    private static readonly HashSet<string> AllowedImageContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    private static readonly HashSet<string> AllowedCvExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf",
        ".doc",
        ".docx"
    };

    private static readonly HashSet<string> AllowedCvContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    };

    private const long MaxImageFileSize = 5 * 1024 * 1024;
    private const long MaxCvFileSize = 10 * 1024 * 1024;

    private readonly IAdminContentService _adminContentService;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AdminController(
        IAdminContentService adminContentService,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _adminContentService = adminContentService;
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("admin-login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var configuredPassword = _configuration["Admin:Password"];
        if (string.IsNullOrWhiteSpace(configuredPassword) || model.Password != configuredPassword)
        {
            ModelState.AddModelError(string.Empty, "Invalid admin password.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "Portfolio Admin"),
            new(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View(await _adminContentService.GetDashboardAsync());
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveProfile(
        [Bind(Prefix = "Profile")] Profile profile,
        IFormFile? profileImage,
        IFormFile? cvFile)
    {
        if (ModelState.IsValid)
        {
            try
            {
                profile.PhotoUrl = await SaveUploadedFileAsync(
                    profileImage,
                    "profile-images",
                    AllowedImageExtensions,
                    AllowedImageContentTypes,
                    MaxImageFileSize,
                    "Profile image") ?? profile.PhotoUrl;
                profile.CvUrl = await SaveUploadedFileAsync(
                    cvFile,
                    "cv",
                    AllowedCvExtensions,
                    AllowedCvContentTypes,
                    MaxCvFileSize,
                    "CV") ?? profile.CvUrl;
                await _adminContentService.SaveProfileAsync(profile);
                TempData["AdminMessage"] = "About section saved.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["AdminError"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["AdminError"] = $"About section could not be saved. {ex.GetBaseException().Message}";
            }
        }
        else
        {
            TempData["AdminError"] = $"About section could not be saved. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddExperience(Experience experience)
    {
        if (ModelState.IsValid)
        {
            await _adminContentService.AddExperienceAsync(experience);
            TempData["AdminMessage"] = "Experience added.";
        }
        else
        {
            TempData["AdminError"] = $"Experience could not be added. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteExperience(int id)
    {
        await _adminContentService.DeleteExperienceAsync(id);
        TempData["AdminMessage"] = "Experience deleted.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateExperience(Experience experience)
    {
        if (ModelState.IsValid)
        {
            await _adminContentService.UpdateExperienceAsync(experience);
            TempData["AdminMessage"] = "Experience saved.";
        }
        else
        {
            TempData["AdminError"] = $"Experience could not be saved. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProject(Project project, IFormFile? projectImage)
    {
        if (ModelState.IsValid)
        {
            try
            {
                project.ImageUrl = await SaveUploadedFileAsync(
                    projectImage,
                    "project-images",
                    AllowedImageExtensions,
                    AllowedImageContentTypes,
                    MaxImageFileSize,
                    "Project image") ?? project.ImageUrl;
                await _adminContentService.AddProjectAsync(project);
                TempData["AdminMessage"] = "Project added.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["AdminError"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["AdminError"] = $"Project could not be added. {ex.GetBaseException().Message}";
            }
        }
        else
        {
            TempData["AdminError"] = $"Project could not be added. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProject(int id)
    {
        await _adminContentService.DeleteProjectAsync(id);
        TempData["AdminMessage"] = "Project deleted.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProject(Project project, IFormFile? projectImage)
    {
        if (ModelState.IsValid)
        {
            try
            {
                project.ImageUrl = await SaveUploadedFileAsync(
                    projectImage,
                    "project-images",
                    AllowedImageExtensions,
                    AllowedImageContentTypes,
                    MaxImageFileSize,
                    "Project image") ?? project.ImageUrl;
                await _adminContentService.UpdateProjectAsync(project);
                TempData["AdminMessage"] = "Project saved.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["AdminError"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["AdminError"] = $"Project could not be saved. {ex.GetBaseException().Message}";
            }
        }
        else
        {
            TempData["AdminError"] = $"Project could not be saved. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReorderProjects([FromForm] List<int> projectIds)
    {
        if (projectIds.Count == 0)
        {
            return BadRequest(new { message = "No projects were sent." });
        }

        await _adminContentService.ReorderProjectsAsync(projectIds);
        return Ok(new { message = "Project order saved." });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEducation(Education education)
    {
        ValidateEducationGrade(education);

        if (ModelState.IsValid)
        {
            await _adminContentService.AddEducationAsync(education);
            TempData["AdminMessage"] = "Education added.";
        }
        else
        {
            TempData["AdminError"] = $"Education could not be added. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEducation(int id)
    {
        await _adminContentService.DeleteEducationAsync(id);
        TempData["AdminMessage"] = "Education deleted.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateEducation(Education education)
    {
        ValidateEducationGrade(education);

        if (ModelState.IsValid)
        {
            await _adminContentService.UpdateEducationAsync(education);
            TempData["AdminMessage"] = "Education saved.";
        }
        else
        {
            TempData["AdminError"] = $"Education could not be saved. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    private void ValidateEducationGrade(Education education)
    {
        if (string.IsNullOrWhiteSpace(education.Grade))
        {
            ModelState.AddModelError(nameof(Education.Grade), "GPA is required.");
        }
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSkill(Skill skill)
    {
        if (ModelState.IsValid)
        {
            await _adminContentService.AddSkillAsync(skill);
            TempData["AdminMessage"] = "Skill added.";
        }
        else
        {
            TempData["AdminError"] = $"Skill could not be added. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        await _adminContentService.DeleteSkillAsync(id);
        TempData["AdminMessage"] = "Skill deleted.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateSkill(Skill skill)
    {
        if (ModelState.IsValid)
        {
            await _adminContentService.UpdateSkillAsync(skill);
            TempData["AdminMessage"] = "Skill saved.";
        }
        else
        {
            TempData["AdminError"] = $"Skill could not be saved. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCertification(Certification certification)
    {
        if (ModelState.IsValid)
        {
            await _adminContentService.AddCertificationAsync(certification);
            TempData["AdminMessage"] = "Certification added.";
        }
        else
        {
            TempData["AdminError"] = $"Certification could not be added. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCertification(int id)
    {
        await _adminContentService.DeleteCertificationAsync(id);
        TempData["AdminMessage"] = "Certification deleted.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCertification(Certification certification)
    {
        if (ModelState.IsValid)
        {
            await _adminContentService.UpdateCertificationAsync(certification);
            TempData["AdminMessage"] = "Certification saved.";
        }
        else
        {
            TempData["AdminError"] = $"Certification could not be saved. {GetModelStateErrors()}";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/#about");
    }

    private string GetModelStateErrors()
    {
        var errors = ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .SelectMany(entry => entry.Value!.Errors.Select(error =>
                $"{entry.Key}: {error.ErrorMessage}"))
            .ToList();

        return errors.Count == 0
            ? "Please check the required fields."
            : string.Join(" ", errors);
    }

    private async Task<string?> SaveUploadedFileAsync(
        IFormFile? file,
        string folderName,
        IReadOnlySet<string> allowedExtensions,
        IReadOnlySet<string> allowedContentTypes,
        long maxFileSize,
        string label)
    {
        if (file is null || file.Length == 0)
        {
            return null;
        }

        if (file.Length > maxFileSize)
        {
            throw new InvalidOperationException($"{label} is too large. Maximum size is {maxFileSize / 1024 / 1024} MB.");
        }

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !allowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException($"{label} type is not allowed.");
        }

        if (!allowedContentTypes.Contains(file.ContentType))
        {
            throw new InvalidOperationException($"{label} content type is not allowed.");
        }

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        var asset = new UploadedAsset
        {
            FileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}{extension}",
            ContentType = file.ContentType,
            Data = memory.ToArray()
        };

        _context.UploadedAssets.Add(asset);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException(
                $"File could not be saved to the database. {ex.GetBaseException().Message}",
                ex);
        }

        return Url.Action("Get", "Files", new { id = asset.Id }) ?? $"/files/{asset.Id}";
    }
}
