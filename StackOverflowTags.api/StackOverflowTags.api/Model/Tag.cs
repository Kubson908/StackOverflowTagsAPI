using System.ComponentModel.DataAnnotations;

namespace StackOverflowTags.api.Model;

public class Tag
{
    [Key]
    public int? Id { get; set; } = null;
    [Required]
    public bool Has_synonyms { get; set; }
    [Required]
    public bool Is_moderator_only { get; set; }
    [Required]
    public bool Is_required { get; set; }
    [Required]
    public int Count { get; set; }
    [Required]
    public required string Name { get; set; }
    public double? PopulationPercentage { get; set; } = null;
}