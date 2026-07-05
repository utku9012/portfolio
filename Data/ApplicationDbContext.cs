using Microsoft.EntityFrameworkCore;
using portfolio.Models.Entities;

namespace portfolio.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Certification> Certifications => Set<Certification>();
    public DbSet<UploadedAsset> UploadedAssets => Set<UploadedAsset>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Profile>().Property(profile => profile.Email).HasMaxLength(160);
        modelBuilder.Entity<Profile>().Property(profile => profile.Phone).HasMaxLength(80);
        modelBuilder.Entity<Profile>().Property(profile => profile.Location).HasMaxLength(160);
        modelBuilder.Entity<Profile>().Property(profile => profile.PhotoUrl).HasMaxLength(500);
        modelBuilder.Entity<Profile>().Property(profile => profile.CvUrl).HasMaxLength(500);
        modelBuilder.Entity<Profile>().Property(profile => profile.TitleTr).HasMaxLength(200);

        modelBuilder.Entity<Experience>().Property(experience => experience.Company).HasMaxLength(160);
        modelBuilder.Entity<Experience>().Property(experience => experience.RoleTr).HasMaxLength(200);
        modelBuilder.Entity<Project>().Property(project => project.Name).HasMaxLength(160);
        modelBuilder.Entity<Project>().Property(project => project.NameTr).HasMaxLength(160);
        modelBuilder.Entity<Project>().Property(project => project.ImageUrl).HasMaxLength(500);
        modelBuilder.Entity<Project>().Property(project => project.DisplayOrder).HasDefaultValue(0);
        modelBuilder.Entity<Education>().Property(education => education.School).HasMaxLength(160);
        modelBuilder.Entity<Education>().Property(education => education.SchoolTr).HasMaxLength(160);
        modelBuilder.Entity<Education>().Property(education => education.DegreeTr).HasMaxLength(200);
        modelBuilder.Entity<Education>().Property(education => education.Description).IsRequired(false);
        modelBuilder.Entity<Skill>().Property(skill => skill.Name).HasMaxLength(100);
        modelBuilder.Entity<Skill>().Property(skill => skill.NameTr).HasMaxLength(100);
        modelBuilder.Entity<Certification>().Property(certification => certification.Name).HasMaxLength(180);
        modelBuilder.Entity<Certification>().Property(certification => certification.NameTr).HasMaxLength(180);
        modelBuilder.Entity<Certification>().Property(certification => certification.Issuer).HasMaxLength(160);
        modelBuilder.Entity<Certification>().Property(certification => certification.IssuerTr).HasMaxLength(160);
        modelBuilder.Entity<Certification>().Property(certification => certification.CredentialUrl).HasMaxLength(500);

        modelBuilder.Entity<UploadedAsset>().Property(asset => asset.FileName).HasMaxLength(255);
        modelBuilder.Entity<UploadedAsset>().Property(asset => asset.ContentType).HasMaxLength(120);
    }
}
