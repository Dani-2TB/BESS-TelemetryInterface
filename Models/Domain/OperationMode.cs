using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Models.Domain;

public class OperationMode
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = "noname";
}