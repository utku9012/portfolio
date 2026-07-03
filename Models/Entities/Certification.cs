using System.ComponentModel.DataAnnotations;

namespace portfolio.Models.Entities;

public class Certification : IEntity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? NameTr { get; set; }

    public string? Issuer { get; set; }

    public string? IssuerTr { get; set; }

    [Display(Name = "Issue date")]
    public string? IssueDate { get; set; }

    [Display(Name = "Issue date TR")]
    public string? IssueDateTr { get; set; }

    [Url, Display(Name = "Credential URL")]
    public string? CredentialUrl { get; set; }

    public string? Description { get; set; }

    public string? DescriptionTr { get; set; }
}
