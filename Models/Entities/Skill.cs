using System.ComponentModel.DataAnnotations;

namespace portfolio.Models.Entities;

public class Skill : IEntity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? NameTr { get; set; }
}
